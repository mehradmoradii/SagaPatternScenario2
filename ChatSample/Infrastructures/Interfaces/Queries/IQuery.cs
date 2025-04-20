using MediatR;

namespace ChatSample.Infrastructures.Interfaces.Queries
{
    public interface IQuery : IRequest
    {
    }

    public interface IQuery<TDto> : IRequest<TDto> where TDto : class
    {

    }
}
