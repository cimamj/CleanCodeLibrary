using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Persistance.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class GetUsedGenresRequest { }
    
 public class GetUsedGenresRequestHandler : RequestHandler<GetUsedGenresRequest, List<string>>
{
        private readonly IBookRepository _bookRepository;
    private readonly ICacheService<List<string>> _cache; //tocno to ne dto

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
            TimeSpan.FromDays(1)  // nikad se ne minja
        );

        //var newgenres = genres.Select(g => g.ToString()).ToList();

        result.SetResult(genres);
        return result;
    }

        protected override Task<bool> IsAuthorized()
        {
            return Task.FromResult(true);
        }
    }
}
