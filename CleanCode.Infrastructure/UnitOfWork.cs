using CleanCode.Infrastructure.Database;
using CleanCodeLibrary.Domain.Persistance.Common;
using Microsoft.EntityFrameworkCore.Storage; //storage radi _transaction
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCode.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    { //ukoliko imamo transakcije
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
            //da spremamo u bazu ako nemamo errora
            await _dbContext.SaveChangesAsync();
        }
    }
}
