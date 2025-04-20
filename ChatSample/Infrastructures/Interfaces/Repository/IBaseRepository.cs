using System.Linq.Expressions;

namespace ChatSample.Infrastructures.Interfaces.Repository
{
    public interface IBaseRepository<T, Tkey> where T : class
    {
        IQueryable<T> FindAll();
        T FindById(Guid id);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition);
        Task Create(T entity);
        Task Update(T entity, Tkey id);
        Task Delete(T entity);
        //Task DeleteAll();
        Task Save();
    

    }
}
