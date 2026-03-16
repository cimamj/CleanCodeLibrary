using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Entities.Borrows;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Common;
using System.Runtime.CompilerServices;


namespace CleanCodeLibrary.Application.Borrows.Borrow
{
    public class BorrowBookRequest
    {
        public int StudentId { get; set; }
        public int BookId { get; set; }
        public DateTime DueDate { get; set; }

        public int Amount { get; set; }
    }
    public class BorrowBookRequestHandler : RequestHandler<BorrowBookRequest, SuccessPostResponse>
    {
        public IBorrowUnitOfWork _unitOfWork;
        private readonly ICacheService<GetAllResponse<BookDto>> _cache;
        public BorrowBookRequestHandler(IBorrowUnitOfWork unitOfWork, ICacheService<GetAllResponse<BookDto>> cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }


        protected override async Task<Result<SuccessPostResponse>> HandleRequest(
    BorrowBookRequest request,
    Result<SuccessPostResponse> result)
        {
         

                var borrow = new CleanCodeLibrary.Domain.Entities.Borrows.Borrow
                {
                    StudentId = request.StudentId,
                    BookId = request.BookId,
                    DueDate = DateOnly.FromDateTime(request.DueDate), //ponisti datetime
                    AmountBorrowed = request.Amount
                };

                var domainResult = await borrow.BorrowBook(_unitOfWork);

                result.SetValidationResult(domainResult.ValidationResult);

                if (result.HasError)
                    return result;

                await _unitOfWork.SaveAsync();

            //_cache.Invalidate(CacheKeys.AllBooks);
            //_cache.Invalidate(CacheKeys.TopBooks10);


            result.SetResult(new SuccessPostResponse(borrow.Id));
                return result;
            
           
        }


        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
