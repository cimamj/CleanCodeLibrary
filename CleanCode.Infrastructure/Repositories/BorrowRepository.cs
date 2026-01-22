
using CleanCode.Infrastructure.Database;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Entities.Borrows;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using Microsoft.EntityFrameworkCore;

namespace CleanCode.Infrastructure.Repositories
{
    public class BorrowRepository : Repository<Borrow, int>, IBorrowRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDapperManager _dapperManager;

        public BorrowRepository(DbContext dbContext, IDapperManager dapperManager)
            : base(dbContext)
        {
            _dapperManager = dapperManager ?? throw new ArgumentNullException(nameof(dapperManager));

            _dbContext = dbContext as ApplicationDbContext
                ?? throw new ArgumentException("DbContext must be ApplicationDbContext");
        }

        public async Task<Borrow> GetById(int id)
        {
            return await _dbContext.Borrows.FindAsync(id);
        }

        public async Task<bool> IsBookCurrentlyBorrowed(int bookId)
        {
            return await _dbContext.Borrows.AnyAsync(b => b.BookId == bookId && b.ReturnDate == null); //any ili firstordefault koja je razlika
           //any vraca bool
        }
    }
}
