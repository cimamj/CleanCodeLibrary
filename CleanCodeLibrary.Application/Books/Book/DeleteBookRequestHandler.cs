
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.Persistance.Students;



namespace CleanCodeLibrary.Application.Books.Book
{
    public class DeleteBookRequest
    {
        public int Id { get; set; }
    }
    public class DeleteBookRequestHandler : RequestHandler<DeleteBookRequest, SuccessDeleteResponse>
    {
        public IBookRepository _bookRepository { get; set; }
        public DeleteBookRequestHandler(IBookRepository bookRepository) 
        {
            _bookRepository = bookRepository;
        }

        protected async override Task<Result<SuccessDeleteResponse>> HandleRequest(DeleteBookRequest request, Result<SuccessDeleteResponse> result)
        {
            var domainResult = await CleanCodeLibrary.Domain.Entities.Books.Book.Delete(_bookRepository, request.Id); //jel uredu ovakva static fja, ne moram uvijek samo repository slat u domain?!
            result.SetValidationResult(domainResult.ValidationResult);
            if (result.HasError)
            {
                return result;
            }


            await _bookRepository.SaveAsync(); //moram jer zovem static kako cu drukcije?? pristup bez instaciranja domain entiteta

            result.SetResult(new SuccessDeleteResponse(domainResult.Value));
            return result;
        }

        protected override Task<bool> IsAuthorized()
        {
            return Task.FromResult(true);
        }
    }
}
