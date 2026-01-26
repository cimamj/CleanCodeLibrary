using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.DTOs.Books;


namespace CleanCodeLibrary.Application.Books.Book
{
    public class GetByIdRequest
    {
        public int Id { get; set; }
    }

    //public class BookResponse
    //{
    //    public BookDto _book { get; set; } //ovaj tip je na istoj razini u getallbooks

    //    public BookResponse(BookDto Book) 
    //    {
    //        _book = Book;   
    //    }

    //    public BookResponse() { }
    //}


    public class GetByIdRequestHandler : RequestHandler<GetByIdRequest, BookDto>
    {
        public IBookRepository _bookRepository { get; set; }
        public GetByIdRequestHandler(IBookRepository bookRepository) 
        {
            _bookRepository = bookRepository;
        }

        protected async override Task<Result<BookDto>> HandleRequest(GetByIdRequest request, Result<BookDto> result)
        {
            //var result = await _bookRepository.GetById(result)
            //var bookFromDomain = await CleanCodeLibrary.Domain.Entities.Books.Book.GetByIdDomainAsync(_bookRepository, request.Id);
            //result.SetValidationResult(bookFromDomain.ValidationResult);

            //if(result.HasError)
            //{
            //    return result;
            //}

            //var bookDto = new BookDto
            //{
            //    Author = bookFromDomain.Value.Author,
            //    Title = bookFromDomain.Value.Title,
            //    Isbn = bookFromDomain.Value.Isbn,
            //    Year = bookFromDomain.Value.Year,
            //    Genre = bookFromDomain.Value.Genre
            //};

            //result.SetResult(new BookResponse(bookDto));
            //return result;

            var bookRepoResult = await _bookRepository.GetById(request.Id); //validacija u domainu, a idem direkt na repository, validaciju cu dodat onda ode
            if(bookRepoResult == null)
            {
                result.AddError(new ValidationResultItem { Message = "Knjiga ne postoji" }); //validacija dodana u app sloju
                return result;
            }
            result.SetResult(bookRepoResult);

            return result;

        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);

    }
}
