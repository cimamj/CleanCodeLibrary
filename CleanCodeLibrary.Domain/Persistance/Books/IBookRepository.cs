using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Books;
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
       
    }
}
