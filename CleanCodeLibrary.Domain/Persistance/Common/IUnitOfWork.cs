using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Students;

namespace CleanCodeLibrary.Domain.Persistance.Common
{
    public interface IUnitOfWork
    {
        Task CreateTransaction();

        Task Commit();

        Task Rollback();

        Task SaveAsync();
    }
}
