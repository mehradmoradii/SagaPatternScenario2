using MediatR;

namespace ChatSample.Infrastructures.Interfaces.Command
{
    public interface ICommandHandle<TCommand> :IRequestHandler<TCommand> where TCommand : ICommand
    {
    }

    public interface ICommandHandler<TCommand,  TResponse> : IRequestHandler<TCommand, TResponse> where TCommand:ICommand<TResponse> where TResponse:class { }
    
}
