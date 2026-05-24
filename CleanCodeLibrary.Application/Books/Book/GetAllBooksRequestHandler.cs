using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Persistance.Books;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class GetAllBooksRequest
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }

    public class GetAllBooksRequestHandler : RequestHandler<GetAllBooksRequest, PagedResponse<BookDto>>
    {
        private readonly IBookRepository _bookRepository;

        private readonly IBookCacheService _cache;

        private readonly ICacheService<TotalCount> _cacheGeneric;

        public GetAllBooksRequestHandler(IBookRepository bookRepository, IBookCacheService cache, ICacheService<TotalCount> cacheGeneric)
        {
            _bookRepository = bookRepository;
            _cache = cache;
            _cacheGeneric = cacheGeneric;
        }

        protected override async Task<Result<PagedResponse<BookDto>>> HandleRequest(
            GetAllBooksRequest request,
            Result<PagedResponse<BookDto>> result)
        {
            var books = await _bookRepository.GetAllPagedAsync(request.PageNumber, request.PageSize);

            var totalCountCache = await _cacheGeneric.GetOrSetAsync(
                Keys.TotalCountKey,
                () => _bookRepository.GetTotalCountAsync(),
                TimeSpan.FromMinutes(10)
            );

            if (books.Count == 0)
            {
                result.AddWarning(new ValidationResultItem
                {
                    Message = "Nema knjiga u bazi",
                    ValidationSeverity = ValidationSeverity.Warning,
                });
            }

            var paged = new PagedResponse<BookDto>
            {
                Values = books,
                TotalCount = totalCountCache.value,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            result.SetResult(paged);

            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
