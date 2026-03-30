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
        //public string Title {  get; init; }
        //public string Author { get; set; }
        public string Isbn { get; init;} //oboje init , request opceinto ne minja polja
        //public int Year { get; set; }
        //public GenresEnum Genre { get; init; }

        public int Amount { get; init; } 
    }
    public class CreateBookRequestHandler : RequestHandler<CreateBookRequest, SuccessPostResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBookExternalService _externalService; //apstrakcija briga me implementacija servisa, lako se zamini u DI da ode drugi nastupi 
        

        public CreateBookRequestHandler(IBookRepository bookRepository, ICurrentUserService currentUserService, IBookExternalService externalService)
        {
            _bookRepository = bookRepository;
            _currentUserService = currentUserService;
            _externalService = externalService; //dodaj u DI tj controler
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

            var externalBookDto = await _externalService.GetBookByIsbnAsync(request.Isbn); 
            if (externalBookDto == null)
            {
                result.AddError(new ValidationResultItem
                {
                    Code = "NoExternalBook",
                    Message = "Knjiga s ovim isbn-om ne postoji",
                    ValidationSeverity = ValidationSeverity.Error,
                    ValidationType = ValidationType.NotFound,
                });
                return result;
            }

            //mapiranje i poziv domain validacija i injekt u bazu
            var book = new BookEntity
            {
                Title = externalBookDto.Title,
                Author = externalBookDto.Author,
                Isbn = request.Isbn,
                Year = externalBookDto.Year ?? DateTime.UtcNow.Year,
                Genre = externalBookDto.Genre, 
                Amount = request.Amount,
                BorrowCount = 0
            };

            var validationResult = await book.Create(_bookRepository); //uvik ZABORAVIS await
         
            result.SetValidationResult(validationResult.ValidationResult); 
            if(result.HasError)
                return result;

            await book.SaveChanges(_bookRepository); 

            result.SetResult(new SuccessPostResponse(book.Id));
            return result;
        }

        protected override Task<bool> IsAuthorized()
        {
            var isAdmin = _currentUserService.GetRole() == "Admin";
            return Task.FromResult(isAdmin);
        }
    }
}

