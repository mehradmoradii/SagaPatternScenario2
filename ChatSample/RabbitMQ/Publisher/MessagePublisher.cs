using ChatSample.Infrastructures.Interfaces.RabbitMQ;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Threading.Channels;

namespace ChatSample.RabbitMQ.Publisher
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IChannel _channel;
        private readonly IConnectionFactory _connectionFactory;

        public MessagePublisher(IConfiguration conf)
        {
            _configuration = conf;

            _connectionFactory = new ConnectionFactory()
            {
                HostName = _configuration["HostName"],
                UserName = _configuration["UserName"],
                Password = _configuration["Password"],
                VirtualHost = _configuration["VirtualHost"]
            };

            
        }



        public async void PublisheMessage(object message, string requestQueueName, string replyQueueName)
        {
            _connection = await _connectionFactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(queue: requestQueueName, durable:true, exclusive:false, autoDelete:false);

            await _channel.ExchangeDeclareAsync(exchange: $"{requestQueueName}-Exchange", type: ExchangeType.Direct);

            await _channel.QueueBindAsync(queue:requestQueueName, exchange:$"{requestQueueName}-Exchange",routingKey:requestQueueName);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            var messageProperties = new BasicProperties()
            {
                ReplyTo = replyQueueName,
                CorrelationId = Guid.NewGuid().ToString(),
            };


            await _channel.BasicPublishAsync(exchange: "",
                                 routingKey: requestQueueName,
                                 mandatory:true,
                                 basicProperties: messageProperties,
                                 body: body);


        }
    }
}
