using ChatSample.Infrastructures.Interfaces.Repository;
using ChatSample.Modules.Context;
using System.Linq.Expressions;

namespace ChatSample.Repository
{
    public class BaseRepostory<T, Tkey> : IBaseRepository<T, Tkey> where T : class
    {
        private readonly ChatDbContext _context;

        public BaseRepostory(ChatDbContext context)
        {
            _context = context;
            
        }


        public async Task Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task Delete(T entity)
        {
            _context?.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindAll()
        {
            var data = _context.Set<T>();
            return data;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition)
        {
            var data = _context.Set<T>().Where(condition);
            return data;
        }

        public T FindById(Guid id)
        {
            var data = _context.Set<T>().Find(id);

            if (data != null)
            {
                return data;
            }

            throw new Exception(message: "this message is from RepositoryBase : object is not exists!");
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity, Tkey id)
        {
            var obj = _context.Set<T>().Find(id);

            if (obj != null)
            {
                _context.Entry(obj).CurrentValues.SetValues(entity);
            }
            else
            {
                _context.Set<T>().Update(entity);
            }
        }
    }
}
