using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCode.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CleanCode.Infrastructure.Repositories
{
    public class BookRepository : Repository<Book, int>, IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDapperManager _dapperManager;

        public BookRepository(ApplicationDbContext dbContext, IDapperManager dapperManager)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _dapperManager = dapperManager;
        }

        public async Task<BookDto?> GetById(int id)
        {
            return await _dbContext.Books
                .Where(x => x.Id == id)
                .Select(x => new BookDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    Isbn = x.Isbn,
                    Genre = x.Genre,
                    Year = x.Year,
                    Amount = x.Amount,
                    BorrowCount = x.BorrowCount
                })
                .SingleOrDefaultAsync();
        }

        public async Task<Book> GetByTitle(string title)
        {
            return await _dbContext.Books.FirstOrDefaultAsync(b => b.Title == title);
        }

        public async Task<BookTitleDto?> GetNameById(int id)
        {
            return await _dbContext.Books
                .Where(x => x.Id == id)
                .Select(x => new BookTitleDto { Title = x.Title })
                .SingleOrDefaultAsync();
        }

        public async Task<Book?> GetByIdEntity(int id)
        {
            return await _dbContext.Books
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<List<string>> GetUsedGenresAsync()
        {
            return await _dbContext.Books
                .Select(x => x.Genre.ToString())
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<BookDto>> GetTopBooksByBorrowCountAsync(int count)
        {
            return await _dbContext.Books
                .OrderByDescending(b => b.BorrowCount)
                .Take(count)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    BorrowCount = b.BorrowCount
                })
                .ToListAsync();
        }

        public async Task<bool> IsbnExists(string isbn)
        {
            return await _dbContext.Books.AnyAsync(b => b.Isbn == isbn);
        }

        public async Task<bool> IsbnExistsForOtherBook(string isbn, int currentBookId)
        {
            return await _dbContext.Books
                .AnyAsync(b => b.Isbn == isbn && b.Id != currentBookId);
        }

        public async Task DecrementAmount(int bookId, int amount)
        {
            var book = await _dbContext.Books.FindAsync(bookId);

            book.Amount -= amount;

            _dbContext.Books.Update(book);
        }

        public async Task IncrementAmount(int bookId, int amount)
        {
            var book = await _dbContext.Books.FindAsync(bookId);

            book.Amount += amount;

            _dbContext.Books.Update(book);
        }

        public async Task IncrementBorrowCount(int bookId, int amount)
        {
            var book = await _dbContext.Books.FindAsync(bookId);

            book.BorrowCount += amount;

            _dbContext.Books.Update(book);
        }

        public async Task<List<BookDto>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            const string dataSql = @"
                SELECT Id, Title, Author, BorrowCount,  Isbn, Year, Genre, Amount
                FROM Books
                ORDER BY Title
                LIMIT @PageSize
                OFFSET @Offset;";

            var books = (await _dapperManager.QueryAsync<BookDto>(dataSql, new
            {
                PageSize = pageSize,
                Offset = (pageNumber - 1) * pageSize
            })).ToList();

            return books;
        }

        public async Task<TotalCount> GetTotalCountAsync()
        {
            const string sql = "SELECT COUNT(*) FROM Books";

            var totalCount = await _dapperManager.QuerySingleAsync<int>(sql);

            return new TotalCount(totalCount);
        }
    }
}
