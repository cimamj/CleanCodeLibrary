using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;

namespace CleanCodeLibrary.Application.Borrows.Borrow
{
    public class ReturnBookRequest
    {
        public int BorrowId { get; set; }
    }

    public class ReturnBookRequestHandler : RequestHandler<ReturnBookRequest, SuccessResponse>
    {
        private readonly IBorrowUnitOfWork _unitOfWork;
        private readonly ICacheService<GetAllResponse<BookDto>> _cache;

        public ReturnBookRequestHandler(IBorrowUnitOfWork unitOfWork, ICacheService<GetAllResponse<BookDto>> cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        protected override async Task<Result<SuccessResponse>> HandleRequest(ReturnBookRequest request, Result<SuccessResponse> result)
        {
            var borrow = await _unitOfWork.BorrowRepository.GetById(request.BorrowId);

            if (borrow == null)
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Borrow.BorrowNotFound));

                return result;
            }

            var returnValidation = borrow.ValidateReturn();

            result.SetValidationResult(returnValidation);

            if (result.HasError)
                return result;

            await _unitOfWork.BorrowRepository.UpdateBorrow(borrow);

            await _unitOfWork.SaveAsync();

            _cache.Invalidate(Keys.AllBooks);

            _cache.Invalidate(Keys.TopBooks10);

            result.SetResult(new SuccessResponse { IsSuccess = true });

            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
