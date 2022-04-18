using KUSYS_Demo.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace KUSYS_Demo.Data.Repositories
{
    public class GenericRepository<T, C> : IGenericRepository<T, C> where T : class, new() where C : DbContext, new()
    {
        private readonly C _context;

        public GenericRepository(C context)
        {
            _context = context;
        }

        public async Task Create(T entity)
        {
            var item = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public DbContext getContext()
        {
            return _context;
        }

        public async Task<T> CreateAndReturn(T entity)
        {
            var item = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<Boolean> Update(object id, T Entity)
        {
            var entry = await GetById(id);
            _context.Entry(entry).CurrentValues.SetValues(Entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T> GetOneByWhereList(params Expression<Func<T, bool>>[] whereList)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (Expression<Func<T, bool>> element in whereList)
            {
                query = query.Where(element);
            }

            return await query.FirstOrDefaultAsync();
        }
        public async Task<T> GetByManyRelationsCollection(Expression<Func<T, bool>> where, params Expression<Func<T, Object>>[] relations)
        {
            var item = _context.Set<T>();
            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<T, object> new_item = null;
            foreach (Expression<Func<T, object>> element in relations)
            {
                if (new_item is null)
                {
                    new_item = item.Include(element);
                }
                else
                {
                    new_item = new_item.Include(element);
                }
            }
            var item2 = new_item.Where(where);
            return await item2.FirstOrDefaultAsync();
        }
    }
}
