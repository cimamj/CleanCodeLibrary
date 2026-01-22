using CleanCode.Infrastructure.Database;
using CleanCode.Infrastructure.Repositories;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Common;
using CleanCodeLibrary.Domain.Persistance.Students;
using Microsoft.EntityFrameworkCore.Storage; //storage radi _transaction


namespace CleanCode.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    { //ukoliko imamo transakcije
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction _transaction;

        public IStudentRepository StudentRepository { get; }
        public IBookRepository BookRepository { get; }
        public IBorrowRepository BorrowRepository { get; }

        public UnitOfWork(
            ApplicationDbContext dbContext,
            IStudentRepository studentRepository,
            IBookRepository bookRepository,
            IBorrowRepository borrowRepository)
        {
            _dbContext = dbContext;
            StudentRepository = studentRepository;
            BookRepository = bookRepository;
            BorrowRepository = borrowRepository;
        }

        public async Task CreateTransaction()
        { 
            _transaction = await _dbContext.Database.BeginTransactionAsync();   
        }
        public async Task Commit()
        {
            if (_transaction != null)
            { 
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            } 
        }

        public async Task Rollback()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }

        }

        public async Task SaveAsync()
        {
            //da spremamo u bazu ako nemamo errora
            await _dbContext.SaveChangesAsync();
        }
    }
}
