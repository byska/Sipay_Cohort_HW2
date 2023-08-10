using Serilog;
using Sipay_Cohort_HW2.DataAccess.Context;
using Sipay_Cohort_HW2.DataAccess.DataAccess.Abstract;
using Sipay_Cohort_HW2.DataAccess.DataAccess.Concrete;

namespace Sipay_Cohort_HW2.DataAccess.UnitOfWork
{
    public class Uow : IUow
    {
        private readonly HW2DbContext _dbContext;
        public Uow(HW2DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Complete()
        {
            _dbContext.SaveChanges();
        }

        public void CompleteWithTransaction()
        {
            using (var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.SaveChanges();
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    Log.Error(ex, "CompleteWithTransaction");
                }
            }
        }

        IGenericRepository<T> IUow.GetRepository<T>()
        {
            return new GenericRepository<T>(_dbContext);
        }

        
    }
}
