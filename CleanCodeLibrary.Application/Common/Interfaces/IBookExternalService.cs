using CleanCodeLibrary.Domain.DTOs.Books;

namespace CleanCodeLibrary.Application.Common.Interfaces
{
    public interface IBookExternalService
    {
        Task<BookExternalDto?> GetBookByIsbnAsync(string isbn);
    }
}
