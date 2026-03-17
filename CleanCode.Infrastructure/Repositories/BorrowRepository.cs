
using CleanCode.Infrastructure.Database;
using CleanCodeLibrary.Domain.DTOs.Students;
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
        private readonly IDapperManager _dapperManager;

        public BorrowRepository(ApplicationDbContext dbContext, IDapperManager dapperManager)
            : base(dbContext)
        {

            _dbContext = dbContext;
            _dapperManager = dapperManager;
        }


        public async Task<BorrowStatisticsDto> GetBorrowStatisticsDtoForStudentAsync(int studentId)
        {
            const string sql = @"SELECT
            @StudentId AS StudentId,  
            COUNT(*) AS TotalBorrows,
            SUM(b1.AmountBorrowed) AS TotalBooksBorrowed,
            COUNT(DISTINCT b1.BookId) AS IndividualBooks,
            (SELECT bk.title
             FROM borrows b
             JOIN books bk ON b.bookid = bk.id
             WHERE b.studentId = @StudentId
             GROUP BY b.bookid, bk.title
             ORDER BY COUNT(*) DESC
             LIMIT 1) AS MostBorrowedBookTitle,
            (SELECT COUNT(*)
             FROM borrows b
             JOIN books bk ON b.bookid = bk.id
             WHERE b.studentId = @StudentId
             GROUP BY b.bookid, bk.title
             ORDER BY COUNT(*) DESC
             LIMIT 1) AS MostBorrowedCount,
            AVG(b1.ReturnDate - b1.BorrowDate) FILTER (WHERE b1.ReturnDate IS NOT NULL) AS AverageBorrowDays,
            COUNT(*) FILTER (WHERE b1.ReturnDate > b1.DueDate) AS LateReturns
        FROM Borrows b1
        WHERE b1.StudentId = @StudentId";

            var parameters = new
            {
                StudentId = studentId
            };

            return await _dapperManager.QuerySingleAsync<BorrowStatisticsDto>(sql, parameters);
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
