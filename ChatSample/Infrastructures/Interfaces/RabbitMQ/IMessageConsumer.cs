namespace ChatSample.Infrastructures.Interfaces.RabbitMQ
{
    public interface IMessageConsumer
    {
        Task<string> ReturnRecievedMessage(string QueueName);
        void ProccessMessage(string QueueName);
    }
}
