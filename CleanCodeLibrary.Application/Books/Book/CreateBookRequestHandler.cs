using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Persistance.Books;
using BookEntity = CleanCodeLibrary.Domain.Entities.Books.Book;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class CreateBookRequest
    {
        public string Isbn { get; init; }
        public int Amount { get; init; }
    }

    public class CreateBookRequestHandler : RequestHandler<CreateBookRequest, SuccessPostResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBookExternalService _externalService;

        public CreateBookRequestHandler(
            IBookRepository bookRepository,
            ICurrentUserService currentUserService,
            IBookExternalService externalService)
        {
            _bookRepository = bookRepository;
            _currentUserService = currentUserService;
            _externalService = externalService;
        }

        protected override async Task<Result<SuccessPostResponse>> HandleRequest(CreateBookRequest request, Result<SuccessPostResponse> result)
        {
            var externalBookDto = await _externalService.GetBookByIsbnAsync(request.Isbn);
            if (externalBookDto == null)
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Book.ExternalBookNotFound));
                return result;
            }

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

            var validationResult = book.Validate();

            if (!validationResult.HasError && !string.IsNullOrWhiteSpace(book.Isbn))
            {
                var isbnTaken = await _bookRepository.IsbnExistsForOtherBook(book.Isbn, book.Id);

                if (isbnTaken)
                    validationResult.AddValidationItem(ValidationItems.Book.IsbnAlreadyExists);
            }

            result.SetValidationResult(validationResult);

            if (result.HasError)
                return result;

            await _bookRepository.InsertAsync(book);

            await _bookRepository.SaveAsync();

            result.SetResult(new SuccessPostResponse(book.Id));

            return result;
        }

        protected override Task<bool> IsAuthorized()
        {
            return Task.FromResult(_currentUserService.GetRole() == "Admin");
        }
    }
}
