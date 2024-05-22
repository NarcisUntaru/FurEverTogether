using FurEver_Together.Data;
using FurEver_Together.DataModels;
using FurEver_Together.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurEver_Together.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly FurEverTogetherDbContext dbContext;
        protected readonly DbSet<T> dbSet;

        public GenericRepository(FurEverTogetherDbContext context)
        {
            dbContext = context;
            dbSet = dbContext.Set<T>();
        }
        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
        public async Task<T> FindByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }
        public void Add(T entity)
        {
            dbContext.Add(entity);
        }

        public void Delete(T entity)
        {
            dbContext.Remove(entity);
        }

        public virtual IList<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return dbContext.Set<T>().Where(expression).ToList();
        }

        public virtual IList<T> GetAll()
        {
            return dbContext.Set<T>().ToList();
        }

        public virtual T? GetById(int id)
        {
            return dbContext.Set<T>().Find(id);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }
        public virtual async Task<T?> FindAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            dbContext.Update(entity);
        }

        public void DeleteRange(IList<T> entities)
        {
            foreach (var item in entities)
            {
                Delete(item);
            }
        }
        public bool Exists(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return dbContext.Set<T>().Any(expression);
        }
    }
}