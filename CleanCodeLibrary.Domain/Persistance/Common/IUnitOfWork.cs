

using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Students;

namespace CleanCodeLibrary.Domain.Persistance.Common
{
    public interface IUnitOfWork
    {
        IStudentRepository StudentRepository { get; }
        IBookRepository BookRepository { get; }
        IBorrowRepository BorrowRepository { get; }
        Task CreateTransaction(); //kad minjamo vise entiteta, ako padne drugi entitet brise se i drugi entitet, da jedan ne ostane
        Task Commit();
        Task Rollback();
        Task SaveAsync();
    }
}
