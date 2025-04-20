using RabbitMQ.Client.Events;

namespace ChatSample.Infrastructures.Interfaces.RabbitMQ
{
    public interface IRpcClient
    {
        Task<string> CallWithResponse(string message, string requestQueueName, string responseQueueName, int timeout);
        Task OnResponse(object response, BasicDeliverEventArgs ea);
    }
}
