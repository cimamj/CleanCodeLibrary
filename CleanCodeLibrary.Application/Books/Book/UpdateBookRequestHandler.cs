
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.Persistance.Students;


namespace CleanCodeLibrary.Application.Books.Book
{
    public class UpdateBookRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public int Year { get; set; }
        public GenresEnum Genre { get; set; }
        public int Amount { get; set; }
    }
    public class UpdateBookRequestHandler : RequestHandler<UpdateBookRequest, SuccessPostResponse>
    {
        private readonly IBookRepository _bookRepository;

        public UpdateBookRequestHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        protected async override Task<Result<SuccessPostResponse>> HandleRequest(UpdateBookRequest request, Result<SuccessPostResponse> result)
        {

            //var domainResult = await CleanCodeLibrary.Domain.Entities.Books.Book.GetByIdDomainAsync(_bookRepository, request.Id); //ili u domainu tj repository to rjesit automatski ka za delete
            //result.SetValidationResult(domainResult.ValidationResult);

            //if (result.HasWarning)
            //    return result;/*; ne kroz domain nego*/
            var existingBook = await _bookRepository.GetByIdEntity(request.Id);

            if (existingBook == null)
            {
                result.AddError(new ValidationResultItem
                {
                    Code = "Book.NotFound",
                    Message = "Knjiga ne postoji",
                    ValidationSeverity = ValidationSeverity.Error
                });
                return result;
            }

            existingBook.Title = request.Title;
            existingBook.Author = request.Author;
            existingBook.Isbn = request.Isbn;
            existingBook.Year = request.Year;
            existingBook.Genre = request.Genre;
            existingBook.Amount = request.Amount;


            var validationResult = await existingBook.Update(_bookRepository);
            result.SetValidationResult(validationResult.ValidationResult);

            if (result.HasError) //dugo ime je error
                return result;

            await existingBook.SaveChanges(_bookRepository);

            result.SetResult(new SuccessPostResponse(existingBook.Id)); //koji id jel ovi ili novi od req
            return result;
        }
        protected override Task<bool> IsAuthorized() => Task.FromResult(true);

    }
}
