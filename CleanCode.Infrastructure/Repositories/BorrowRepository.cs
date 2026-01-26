
using CleanCode.Infrastructure.Database;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Entities.Borrows;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;

namespace CleanCode.Infrastructure.Repositories
{
    public class BorrowRepository : Repository<Borrow, int>, IBorrowRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BorrowRepository(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext as ApplicationDbContext
                ?? throw new ArgumentException("DbContext must be ApplicationDbContext");
        }

        public async Task<Borrow> GetById(int id)
        {
            return await _dbContext.Borrows.FindAsync(id);
        }

        public async Task<int> InsertBorrow(Borrow borrow, int amount)
        {
            await _dbContext.Borrows.AddAsync(borrow);

            var book = await _dbContext.Books.FindAsync(borrow.BookId);
            book!.Amount -= amount;
            _dbContext.Books.Update(book);
            return borrow.Id;
        }

        public async Task<bool> IsBookCurrentlyBorrowed(int bookId)
        {
            return await _dbContext.Borrows.AnyAsync(b => b.BookId == bookId && b.ReturnDate == null); //any ili firstordefault koja je razlika
           //any vraca bool
        }
    }
}
