using MediatR;

namespace ChatSample.Infrastructures.Interfaces.Command
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<TResponse> : IRequest<TResponse> where TResponse : class
    {

    }
}
