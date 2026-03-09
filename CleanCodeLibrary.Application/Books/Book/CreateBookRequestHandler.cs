using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Persistance.Books;
using BookEntity = CleanCodeLibrary.Domain.Entities.Books.Book;

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
        private readonly ICurrentUserService _currentUserService;

        public CreateBookRequestHandler(IBookRepository bookRepository, ICurrentUserService currentUserService)
        {
            _bookRepository = bookRepository;
            _currentUserService = currentUserService;
        }

        protected async override Task<Result<SuccessPostResponse>> HandleRequest(CreateBookRequest request, Result<SuccessPostResponse> result)
        {
            var role = _currentUserService.GetRole();

            if (role != "Admin")
            {
                result.AddError(new ValidationResultItem
                {
                    Code = "AccessDenied",
                    Message = "Nemate administratorska prava za ovu akciju.",
                    ValidationSeverity = ValidationSeverity.Error,
                    ValidationType = ValidationType.FormalValidation,
                });
                return result; 
            }

            var book = new BookEntity
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
                return result;

            await book.SaveChanges(_bookRepository); //kroz domain ili repo?

            result.SetResult(new SuccessPostResponse(book.Id));
            return result;
        }

        protected override Task<bool> IsAuthorized()
        {
            //var user = _currentUserService.GetRole();
            //if(user != "Admin")
            //    return Task.FromResult(false);
            return Task.FromResult(true);
        }
    }
}

