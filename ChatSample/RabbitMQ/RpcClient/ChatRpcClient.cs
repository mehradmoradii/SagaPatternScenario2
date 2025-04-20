using ChatSample.Infrastructures.Interfaces.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatSample.RabbitMQ.RpcClient
{
    public class ChatRpcClient : IRpcClient
    {
        private IConnection _connection;
        private IChannel _channel;
        private IConnectionFactory _connectionFactory;
        private IConfiguration _configuration;

        private AsyncEventingBasicConsumer _consumer;
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

        public async Task InitAsync()
        {
            _connection = await _connectionFactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            var reply = await _channel.QueueDeclareAsync(
                queue: "",
                durable: false,
                exclusive: true,
                autoDelete: true
            );

            _responseQueue = reply.QueueName;
            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.ReceivedAsync += OnResponse;

            await _channel.BasicConsumeAsync(
                queue: _responseQueue,
                autoAck: true,
                consumer: _consumer
            );
        }

        public async Task<string> CallWithResponse(string message, string requestQueueName, string responseQueueName, int timeout)
        {
            _connection = await _connectionFactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(queue:requestQueueName,durable:true,exclusive:false, autoDelete:false);
            await _channel.ExchangeDeclareAsync(exchange: $"{requestQueueName}-Exchange", type: ExchangeType.Direct);
            await _channel.QueueBindAsync(queue: requestQueueName, exchange: $"{requestQueueName}-Exchange", routingKey: requestQueueName);

            _corrId = Guid.NewGuid().ToString();
            _tcs = new TaskCompletionSource<string>();

            var msgProperties = new BasicProperties()
            {
                CorrelationId = _corrId,
                ReplyTo = _responseQueue
            };

            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(
                exchange: $"{requestQueueName}-Exchange",
                routingKey: requestQueueName,
                mandatory: true,
                basicProperties: msgProperties,
                body: body
            );

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
            await using (cts.Token.Register(() => _tcs.TrySetCanceled()))
            {
                return await _tcs.Task;
            }
        }

        public Task OnResponse(object response, BasicDeliverEventArgs ea)
        {
            if (ea.BasicProperties.CorrelationId == _corrId)
            {
                var body = ea.Body.ToArray();
                var result = Encoding.UTF8.GetString(body);
                _tcs.TrySetResult(result);
            }

            return Task.CompletedTask;
        }
    }
}
