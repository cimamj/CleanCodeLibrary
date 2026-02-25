using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.Persistance.Common;
using CleanCodeLibrary.Domain.Persistance.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeLibrary.Domain.Persistance.Borrows
{
    public interface IBorrowUnitOfWork : IUnitOfWork
    {
        IBookRepository BookRepository { get; }
        IBorrowRepository BorrowRepository { get; }
        IStudentRepository StudentRepository { get; }
    }
}
