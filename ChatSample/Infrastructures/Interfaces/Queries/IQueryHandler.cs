using MediatR;

namespace ChatSample.Infrastructures.Interfaces.Queries
{
    public interface IQueryHandler<TQuery> : IRequestHandler<TQuery> where TQuery : IQuery
    {

        
    }

    public interface IQueryHandler<TQuery, TDto> : IRequestHandler<TQuery, TDto> where TQuery: IQuery<TDto> where TDto:class
    {

    }
}
