using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Persistance.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class GetTopBooksRequest { }
    public class GetTopBooksRequestHandler : RequestHandler<GetTopBooksRequest, List<BookDto>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICacheService<List<BookDto>> _cache;

        public GetTopBooksRequestHandler(IBookRepository bookRepository, ICacheService<List<BookDto>> cache)
        {
            _bookRepository = bookRepository;
            _cache = cache;
        }

        protected override async Task<Result<List<BookDto>>> HandleRequest(GetTopBooksRequest request, Result<List<BookDto>> result)
        {
            var topBooks = await _cache.GetOrSetAsync(
                key: Keys.TopBooks10,
                factory: () => _bookRepository.GetTopBooksByBorrowCountAsync(10),
                expiration: TimeSpan.FromMinutes(10)  // ovo se mijenja cesce od npr genres
            );

            result.SetResult(topBooks);
            return result;
        }

        protected override Task<bool> IsAuthorized()
        {
            return Task.FromResult(true);   
        }
    }
}
