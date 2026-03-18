using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Persistance.Common;


namespace CleanCodeLibrary.Domain.Persistance.Books
{
    public interface IBookRepository : IRepository<Book, int>
    {
        Task<BookDto?> GetById(int id);
        Task<Book> GetByTitle(string title);
        Task<BookTitleDto?> GetNameById(int id);

        Task<Book?> GetByIdEntity(int id);

        Task<GetAllResponse<BookDto>> GetAllBookDtos(); //DOMAIN NEZNA ZA NIKOGA 

        Task<bool> IsbnExists(string isbn); //za create metodu

        Task<bool> IsbnExistsForOtherBook(string isbn, int currentBookId);

        Task DecrementAmount(int bookId, int amount);
        Task IncrementAmount(int bookId, int amount);

        Task IncrementBorrowCount(int  bookId, int amount);

        Task<List<string>> GetUsedGenresAsync();

        Task<List<BookDto>> GetTopBooksByBorrowCountAsync(int count);

        Task<(List<BookDto> Books, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize);
    }
}
