using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCode.Infrastructure
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
    {
        private readonly DbContext _context;

        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;

            _dbSet = context.Set<TEntity>();
        }

        public async Task<GetAllResponse<TEntity>> Get()
        {
            var entities = await _dbSet.ToListAsync();

            return new GetAllResponse<TEntity> { Values = entities };
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<bool> DeleteAsync(TId id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);

                return true;
            }
            else

                return false;
        }

        public void Delete(TEntity? entity)
        {
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
