using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Books;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class GetUsedGenresRequest
    {
    }

    public class GetUsedGenresRequestHandler : RequestHandler<GetUsedGenresRequest, List<string>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICacheService<List<string>> _cache;

        public GetUsedGenresRequestHandler(IBookRepository bookRepository, ICacheService<List<string>> cache)
        {
            _bookRepository = bookRepository;
            _cache = cache;
        }

        protected override async Task<Result<List<string>>> HandleRequest(GetUsedGenresRequest request, Result<List<string>> result)
        {
            var genres = await _cache.GetOrSetAsync(
                Keys.AllGenres,
                () => _bookRepository.GetUsedGenresAsync(),
                TimeSpan.FromDays(1)
            );

            result.SetResult(genres);

            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
