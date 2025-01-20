using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TechXpress_E_commerce.Models.AppDbContext;

namespace TechXpress_E_commerce.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly  AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
          return  _dbSet.ToList();
        }
        public T GetById(int id)
        {
           return _dbSet.Find(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity); 
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity); 
        } 
        public void Delete(T entity)
        { 
            _dbSet.Remove(entity); 
           
        }   
        public void SaveChanges()
        {
             _context.SaveChanges();
        }
    }
}
