

using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;

namespace CleanCodeLibrary.Domain.Persistance.Students
{
    public interface IStudentUnitOfWork
    {
        IStudentRepository StudentRepository { get; }

    }
}
  