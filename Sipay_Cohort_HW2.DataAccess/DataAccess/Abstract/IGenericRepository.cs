using Sipay_Cohort_HW2.Entities;
using System.Linq.Expressions;

namespace Sipay_Cohort_HW2.DataAccess.DataAccess.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> Add(T entity);
        Task<bool> Any(Expression<Func<T, bool>> exp);
        Task<List<T>> GetActive();
        Task<IQueryable<T>> GetActivesAsync(params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync();
        Task<IQueryable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<IQueryable<T>> GetAllByParametersAsync(Expression<Func<T, object>> include,params Expression<Func<T, bool>>[] exps);
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes);
        Task<T> GetByDefault(params Expression<Func<T, bool>>[] exps);
        T GetByID(int id);
        IQueryable<T> GetByID(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetDefault(Expression<Func<T, bool>> exp);
        bool Remove(T entity);
        bool Remove(int id);
        bool Update(T entity);
    }
}
