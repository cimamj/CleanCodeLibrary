

namespace CleanCodeLibrary.Domain.Persistance.Common
{
    public interface IUnitOfWork
    {
        Task CreateTransaction(); //kad minjamo vise entiteta, ako padne drugi entitet brise se i drugi entitet, da jedan ne ostane
        Task Commit();
        Task Rollback();
        Task SaveAsync();
    }
}
