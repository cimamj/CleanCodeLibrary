using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Books;
using Microsoft.Extensions.Caching.Memory;

namespace CleanCode.Api.Services
{
    public class BookCacheService : IBookCacheService
    { //on ima pristup cache
        private readonly IMemoryCache _cache; //ovo se instacira sa DI
        private const string KEY = "all_books";

        public BookCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async Task<GetAllResponse<BookDto>> GetOrSetBooksAsync(Func<Task<GetAllResponse<BookDto>>> factory)
        {
            if (_cache.TryGetValue(KEY, out GetAllResponse<BookDto> cached))
                return cached;

            var result = await factory(); //AWAIT
            _cache.Set(KEY, result, TimeSpan.FromMinutes(5)); //brise se 
            return result;
        }

        public void Invalidate()
        {
            _cache.Remove(KEY);
        }
    }
}
