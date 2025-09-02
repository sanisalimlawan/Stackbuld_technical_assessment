using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IGenericRepo<T> where T : class
    {
        Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate, params string[] includes);
        Task<IEnumerable<T>> GetAllAsync(params string[] includes);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        T Update(T entity);
        T Remove(T entity);
        bool RemoveRange(IEnumerable<T> entities);

        Task<bool> SaveChangesAsync();
    }
}
