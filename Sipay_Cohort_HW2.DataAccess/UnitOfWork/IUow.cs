using Sipay_Cohort_HW2.DataAccess.DataAccess.Abstract;

namespace Sipay_Cohort_HW2.DataAccess.UnitOfWork
{
    public interface IUow
    {
        void Complete();
        void CompleteWithTransaction();
        IGenericRepository<T> GetRepository<T>() where T : class;
    }
}
