using CleanCode.Infrastructure.Database;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCode.Infrastructure.Repositories
{
    public class BorrowUnitOfWork : UnitOfWork, IBorrowUnitOfWork
    {
        //public readonly ApplicationDbContext _dbContext;
        //public IDbContextTransaction _transaction;
        public IBookRepository BookRepository { get; }
        public IBorrowRepository BorrowRepository { get; }
        public IStudentRepository StudentRepository { get; }

        public BorrowUnitOfWork(
            ApplicationDbContext dbContext,
            
            IStudentRepository studentRepository,
            IBookRepository bookRepository,
            IBorrowRepository borrowRepository) : base(dbContext)
        {
            //_dbContext = dbContext;
            //_transaction = transaction;
            StudentRepository = studentRepository;
            BookRepository = bookRepository;
            BorrowRepository = borrowRepository;
        }


    }
}
