using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Persistance.Books;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class UpdateBookRequest
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Author { get; set; }

        public string? Isbn { get; set; }

        public int? Year { get; set; }

        public GenresEnum? Genre { get; set; }

        public int? Amount { get; set; }
    }

    public class UpdateBookRequestHandler : RequestHandler<UpdateBookRequest, SuccessPostResponse>
    {
        private readonly IBookRepository _bookRepository;

        public UpdateBookRequestHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        protected override async Task<Result<SuccessPostResponse>> HandleRequest(UpdateBookRequest request, Result<SuccessPostResponse> result)
        {
            var existingBook = await _bookRepository.GetByIdEntity(request.Id);

            if (existingBook == null)
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Book.NotFound));

                return result;
            }

            if (!string.IsNullOrEmpty(request.Title)) 
                existingBook.Title = request.Title;

            if (!string.IsNullOrEmpty(request.Author)) 
                existingBook.Author = request.Author;

            if (!string.IsNullOrEmpty(request.Isbn)) 
                existingBook.Isbn = request.Isbn;

            if (request.Year.HasValue) 
                existingBook.Year = request.Year.Value;

            if (request.Amount.HasValue) 
                existingBook.Amount = request.Amount.Value;

            if (request.Genre.HasValue) 
                existingBook.Genre = request.Genre.Value;

            var validationResult = existingBook.Validate();

            if (!validationResult.HasError && !string.IsNullOrWhiteSpace(existingBook.Isbn))
            {
                var isbnTaken = await _bookRepository.IsbnExistsForOtherBook(existingBook.Isbn, existingBook.Id);

                if (isbnTaken)
                    validationResult.AddValidationItem(ValidationItems.Book.IsbnAlreadyExists);
            }

            result.SetValidationResult(validationResult);

            if (result.HasError)
                return result;

            _bookRepository.Update(existingBook);

            await _bookRepository.SaveAsync();

            result.SetResult(new SuccessPostResponse(existingBook.Id));

            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
