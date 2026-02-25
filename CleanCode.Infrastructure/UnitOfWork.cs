using CleanCode.Infrastructure.Database;
using CleanCode.Infrastructure.Repositories;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Common;
using CleanCodeLibrary.Domain.Persistance.Students;
using Microsoft.EntityFrameworkCore.Storage; //storage radi _transaction


namespace CleanCode.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    { //ukoliko imamo transakcije
        public readonly ApplicationDbContext _dbContext;
        public IDbContextTransaction _transaction;


        public UnitOfWork(
            ApplicationDbContext dbContext
            )
        {
            _dbContext = dbContext;
            //_transaction = transaction;
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
            Console.WriteLine("SaveAsync called – saving changes...");
            var entries = _dbContext.ChangeTracker.Entries();
            await _dbContext.SaveChangesAsync();
        }
    }
}
