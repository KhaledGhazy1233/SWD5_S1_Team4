using System.Linq.Expressions;

namespace DataLayer.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = null);
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter, string includeProperties = null);
        //T Get(Expression<Func<T, bool>> filter);

        void Add(T entity);
        Task Update(T entity);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entites);
    }
}
