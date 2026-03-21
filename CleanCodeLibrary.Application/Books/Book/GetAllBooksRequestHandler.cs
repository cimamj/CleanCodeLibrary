using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Model; //problem sto sad nezna koji result koristiti, prominit cu ime u domainu resultDomain
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


    public class GetAllBooksRequestHandler //pitaj ivu koji kurac
        : RequestHandler<GetAllBooksRequest, PagedResponse<BookDto>>
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
        //a npr kad mapiran u novu klasu radi

        protected override async Task<Result<PagedResponse<BookDto>>> HandleRequest(
            GetAllBooksRequest request,
            Result<PagedResponse<BookDto>> result)
        {
            //i cachiranje sam maka radi paginacije , kasnie mozda dodat
            //var books = await _cache.GetOrSetBooksAsync(() => _bookRepository.GetAllBookDtos());

            //sad kako iskoristit request? pa slat kroz ovu doli metodu oboje 
            //moram novi objekt vracat, ne samo books pod values, nego i totalCount!
            var books = await _bookRepository.GetAllPagedAsync(request.PageNumber, request.PageSize);

            var totalCountCache = await _cacheGeneric.GetOrSetAsync(
                        Keys.TotalCountKey,
                        () => _bookRepository.GetTotalCountAsync(),
                        TimeSpan.FromMinutes(10)
                        );

            //mozda provjera je li null?

            //var books = await _bookRepository.GetAllBookDtos();
            if (!books.Any()) //validan rezultat, samo warning, ili !Values.Any()
            {
                result.AddWarning(new ValidationResultItem
                {
                    Message = "Nema knjiga u bazi",
                    ValidationSeverity = ValidationSeverity.Warning,
                });
                //return result; OVAKO RESULT NECE IMATI VALUE, UC CE U RESPONSEEXT I VRATIT CE 404, TO NE ZELIMO
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
//GLUPO OPET MAPIRATI NEMA SMISLA NOVI DTO ISPADA, ali konflikt radi result odavde  i iz domaina,
//GetById, GetAll → Repository direktno (nema validacije) ali getbyid mapira u repoziotriju jel to ok? vraca mapirani book, a ode mapiranje isto ali jos jednom mapiran u drugu klasu radi konflikta
//Create, Update, Delete → Domain (ima validaciju)