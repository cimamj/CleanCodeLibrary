using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Persistance.Books;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class DeleteBookRequest
    {
        public int Id { get; set; }
    }

    public class DeleteBookRequestHandler : RequestHandler<DeleteBookRequest, SuccessDeleteResponse>
    {
        private readonly IBookRepository _bookRepository;

        public DeleteBookRequestHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        protected override async Task<Result<SuccessDeleteResponse>> HandleRequest(DeleteBookRequest request, Result<SuccessDeleteResponse> result)
        {
            var bookExists = await _bookRepository.GetByIdEntity(request.Id);

            if (bookExists == null)
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Book.NotFound));

                return result;
            }

            var deleted = await _bookRepository.DeleteAsync(request.Id);

            if (!deleted)
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Book.DeleteWentWrong));

                return result;
            }

            await _bookRepository.SaveAsync();

            result.SetResult(new SuccessDeleteResponse(request.Id));

            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
