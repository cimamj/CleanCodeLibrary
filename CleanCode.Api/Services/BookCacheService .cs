using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Books;
using Microsoft.Extensions.Caching.Memory;

namespace CleanCode.Api.Services
{
    public class BookCacheService : IBookCacheService
    {
        private readonly IMemoryCache _cache;

        private const string KEY = "all_books";

        public BookCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<GetAllResponse<BookDto>> GetOrSetBooksAsync(Func<Task<GetAllResponse<BookDto>>> factory)
        {
            if (_cache.TryGetValue(KEY, out GetAllResponse<BookDto> cached))

                return cached;

            var result = await factory();

            _cache.Set(KEY, result, TimeSpan.FromMinutes(5));

            return result;
        }

        public void Invalidate()
        {
            _cache.Remove(KEY);
        }
    }
}
