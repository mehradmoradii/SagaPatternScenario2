namespace ChatSample.Infrastructures.Interfaces.RabbitMQ
{
    public interface IMessagePublisher
    {
        void PublisheMessage(object message, string requestQueueName, string replyQueueName);
    }
}
