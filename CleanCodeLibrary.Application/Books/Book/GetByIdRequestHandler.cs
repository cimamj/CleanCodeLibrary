using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Persistance.Books;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class GetByIdRequest
    {
        public int Id { get; set; }
    }

    public class GetByIdRequestHandler : RequestHandler<GetByIdRequest, BookDto?>
    {
        private readonly IBookRepository _bookRepository;

        public GetByIdRequestHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        protected override async Task<Result<BookDto?>> HandleRequest(GetByIdRequest request, Result<BookDto?> result)
        {
            var book = await _bookRepository.GetById(request.Id);

            if (book == null)
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Book.NotFound));

                return result;
            }

            result.SetResult(book);

            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
