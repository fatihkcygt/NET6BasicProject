using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KUSYS_Demo.Data.Interfaces
{
    public interface IGenericRepository<T, C> where T : class, new() where C : DbContext, new()
    {
        Task Create(T entity);
        DbContext getContext();
        Task<T> CreateAndReturn(T entity);
        void Remove(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetById(object id);
        Task<Boolean> Update(object id, T Entity);
        Task<T> GetByManyRelationsCollection(Expression<Func<T, bool>> where, params Expression<Func<T, Object>>[] relations);
        Task<T> GetOneByWhereList(params Expression<Func<T, bool>>[] whereList);

    }
}
