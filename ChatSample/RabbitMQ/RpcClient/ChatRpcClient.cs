using ChatSample.Infrastructures.Interfaces.RabbitMQ;
using Microsoft.VisualBasic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;

namespace ChatSample.RabbitMQ.RpcClient
{
    public class ChatRpcClient : IRpcClient, IDisposable
    {
        private IConnection _connection;
        private IChannel _channel;
        private IConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;

        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _pendingRequests = new();
        private TaskCompletionSource<string> _tcs = new();

        private string _corrId;
        private string _responseQueue;

        public ChatRpcClient(IConfiguration conf)
        {
            _configuration = conf;

            _connectionFactory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"],
                VirtualHost = _configuration["RabbitMQ:VirtualHost"]
            };
        }

        public async Task InitAsync(string responseQueueName)
        {
            _connection = await _connectionFactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            var replyQueue = await _channel.QueueDeclareAsync(
                queue: responseQueueName,
                durable: false,
                exclusive: false,
                autoDelete: true
            );

            _responseQueue = replyQueue.QueueName;
            var _consumer = new AsyncEventingBasicConsumer(_channel);


            _consumer.ReceivedAsync += OnResponse;

            await _channel.BasicConsumeAsync(
                queue: _responseQueue,
                autoAck: true,
                consumer: _consumer
            );
        }

        public async Task<string> CallWithResponse(string message, string requestQueueName, string responseQueueName, int timeout)
        {

            if (_connection == null || !_connection.IsOpen)
                _connection = await _connectionFactory.CreateConnectionAsync();

            if (_channel == null || _channel.IsClosed)
                _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(queue: requestQueueName, durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueDeclareAsync(queue: responseQueueName, durable: false, exclusive: false, autoDelete: true);

            _corrId = Guid.NewGuid().ToString();
            _tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            _pendingRequests.TryAdd(_corrId, _tcs);

            var props = new BasicProperties
            {
                CorrelationId = _corrId,
                ReplyTo = responseQueueName
            };

            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: requestQueueName,
                mandatory: true,
                basicProperties: props,
                body: body
            );
            var _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                _tcs.SetResult(json);

            };
            

            await _channel.BasicConsumeAsync(
                queue: responseQueueName,
                autoAck: true,
                consumer: _consumer
            );

            

            

            return await _tcs.Task;

        }

        public Task OnResponse(object response, BasicDeliverEventArgs ea)
        {
            if (ea.BasicProperties.CorrelationId == _corrId)
            {
                var body = ea.Body.ToArray();
                var result = Encoding.UTF8.GetString(body);
                _tcs.TrySetResult(result);
            }

            return _tcs.Task;
        }

        public async void Dispose()
        { 

           await _channel?.CloseAsync();
           await _connection?.CloseAsync();
        }
    }
}
