
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

        public BorrowRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Borrow> GetById(int id)
        {
            return await _dbContext.Borrows.FindAsync(id);
        }

        public async Task<int> InsertBorrow(Borrow borrow, int amount)
        {
            await _dbContext.Borrows.AddAsync(borrow);

           

            DebugChangeTracker(_dbContext);
            return borrow.Id;
        }

        public async Task<bool> IsBookCurrentlyBorrowed(int bookId)
        {
            return await _dbContext.Borrows.AnyAsync(b => b.BookId == bookId && b.ReturnDate == null); //any ili firstordefault koja je razlika
           //any vraca bool
        }

        public async Task UpdateBorrow(Borrow item)
        {
            var entity = await _dbContext
                .Borrows
                .Include(x => x.Book)
                .SingleOrDefaultAsync(x => x.Id == item.Id);

            if (entity == null)
                return;


            entity.ReturnDate = DateOnly.FromDateTime(DateTime.UtcNow);
            entity.Book.Amount += entity.AmountBorrowed;
        }
            
        private void DebugChangeTracker(ApplicationDbContext context)
        {
            Console.WriteLine("===== CHANGE TRACKER =====");
            foreach (var entry in context.ChangeTracker.Entries())
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}");
                Console.WriteLine($"State: {entry.State}");
                if (entry.Entity is Borrow borrow)
                {
                    Console.WriteLine($" Borrow Id: {borrow.Id}");
                    Console.WriteLine($" StudentId: {borrow.StudentId}");
                    Console.WriteLine($" BookId: {borrow.BookId}");
                }
                if (entry.Entity is Book book)
                {
                    Console.WriteLine($" Book Id: {book.Id}");
                    Console.WriteLine($" Amount: {book.Amount}");
                }
                Console.WriteLine("---");
            }
            Console.WriteLine("==========================");
        }
    }
    
    
}
