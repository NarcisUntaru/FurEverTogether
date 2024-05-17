using System.Linq.Expressions;

namespace FurEver_Together.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IList<T> GetAll();

        T? GetById(int id);

        void Update(T entity);

        void Delete(T entity);

        IList<T> Get(Expression<Func<T, bool>> expression);

        void Add(T entity);

        void Save();

        void DeleteRange(IList<T> entities);
    }
}