using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Persistance.Books;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class CreateBookRequest
    {
        public string Title {  get; init; }
        public string Author { get; set; }
        public string Isbn { get; set;}//ocu sve stavit get set ili private set
        public int Year { get; set; }
        public GenresEnum Genre { get; init; }

        public int Amount { get; init; }
    }
    public class CreateBookRequestHandler : RequestHandler<CreateBookRequest, SuccessPostResponse>
    {
        private readonly IBookRepository _bookRepository;

        public CreateBookRequestHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        protected async override Task<Result<SuccessPostResponse>> HandleRequest(CreateBookRequest request, Result<SuccessPostResponse> result)
        {
            var book = new Domain.Entities.Books.Book
            {
                Title = request.Title,
                Author = request.Author,
                Isbn = request.Isbn,
                Year = request.Year,
                Genre = request.Genre ,
                Amount = request.Amount
            };

            var validationResult = await book.Create(_bookRepository); //uvik ZABORAVIS await
         
            result.SetValidationResult(validationResult.ValidationResult); 
            if(result.HasError)
            {
                return result;
            }
            await book.SaveChanges(_bookRepository);

            result.SetResult(new SuccessPostResponse(book.Id));
            return result;
          
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}

