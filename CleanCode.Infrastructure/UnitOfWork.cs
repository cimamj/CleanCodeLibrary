using CleanCode.Infrastructure.Database;
using CleanCodeLibrary.Domain.Persistance.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanCode.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
            await _dbContext.SaveChangesAsync();
        }
    }
}
