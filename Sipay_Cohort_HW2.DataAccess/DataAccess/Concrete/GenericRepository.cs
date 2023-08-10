using Microsoft.EntityFrameworkCore;
using Sipay_Cohort_HW2.DataAccess.Context;
using Sipay_Cohort_HW2.DataAccess.DataAccess.Abstract;
using System.Linq.Expressions;
using System.Transactions;

namespace Sipay_Cohort_HW2.DataAccess.DataAccess.Concrete
{
    public class GenericRepository<T> :IGenericRepository<T> where T : class
    {
        private readonly HW2DbContext _context;
        private readonly DbSet<T> _table;
        public GenericRepository(HW2DbContext context)
        {
            _context = context;
            _table = _context.Set<T>();

        }

        public async Task<bool> Add(T entity)
        {
            try
            {
                await _table.AddAsync(entity);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> Any(Expression<Func<T, bool>> exp) => await _table.AnyAsync(exp);

        public async Task<List<T>> GetActive() => await _table.Where(x => x.IsActive == true).ToListAsync();

        public async Task<IQueryable<T>> GetActivesAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _table.Where(x => x.IsActive == true);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public async Task<List<T>> GetAllAsync() => await _table.ToListAsync();

        public async Task<IQueryable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _table.AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }
        public async Task<IQueryable<T>> GetAllByParametersAsync(Expression<Func<T, object>> include, params Expression<Func<T, bool>>[] exps)
        {
            
            IQueryable<T> query = _table.AsQueryable();
            query = query.Include(include);
            var queryList= query.ToList();
            foreach (var exp in exps)
            {
                query = query.Where(exp);
            }
            queryList = query.ToList();
            
            return query;
        }

        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes)
        {
            var query = _table.Where(exp);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public async Task<T> GetByDefault(params Expression<Func<T, bool>>[] exps) 
        {
            IQueryable<T> query = _table.AsQueryable();

            foreach (var exp in exps)
            {
                query = query.Where(exp);
            }
            return query.FirstOrDefault();
        }

        public T GetByID(int id) => _table.Find(id);

        public IQueryable<T> GetByID(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _table.Where(x => x.Id == id);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public async Task<IEnumerable<T>> GetDefault(Expression<Func<T, bool>> exp) => await _table.Where(exp).ToListAsync();

        public bool Remove(T entity)
        {
            entity.IsActive = false;
            return Update(entity);
        }

        public bool Remove(int id)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    T item = GetByID(id);
                    item.IsActive = false;
                    ts.Complete();
                    return Update(item);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(T entity)
        {
            try
            {
                _table.Update(entity);
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
