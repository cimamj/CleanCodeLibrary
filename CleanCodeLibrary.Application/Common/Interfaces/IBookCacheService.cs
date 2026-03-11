

using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Books;

namespace CleanCodeLibrary.Application.Common.Interfaces
{
    public interface IBookCacheService
    {
        Task<GetAllResponse<BookDto>> GetOrSetBooksAsync(Func<Task<GetAllResponse<BookDto>>> factory);
        void Invalidate();
    }
}
