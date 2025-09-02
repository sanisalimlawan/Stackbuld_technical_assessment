using Core.Interface;
using Infrastracture.Data;
using Infrastracture.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Services
{
    public class GenericService<T> : IGenericRepo<T> where T : class
    {
        private readonly MyDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericService(MyDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate, params string[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(predicate);

        }

        public async Task<IEnumerable<T>> GetAllAsync(params string[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Apply each include
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entities = await query
                .ToListAsync();
            return entities;

        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var entities = await _dbSet.Where(predicate).ToListAsync();
            return entities;

        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            
            return entity;

        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;

        }

        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;

        }

        public T Remove(T entity)
        {
            _dbSet.Remove(entity);
            
                return entity;
        }



        public bool RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return true;

        }


        public async Task<bool> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();

            return true;

        }


    }
}
