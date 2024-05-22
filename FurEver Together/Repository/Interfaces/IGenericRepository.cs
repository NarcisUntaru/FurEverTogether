using System.Linq.Expressions;

namespace FurEver_Together.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task SaveAsync();
        Task<T?> FindAsync(int id);
        IList<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        bool Exists(Expression<Func<T, bool>> expression); 
        void Update(T entity);
        void Delete(T entity);
        IList<T> Get(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void Save();
        void DeleteRange(IList<T> entities);
    }
}