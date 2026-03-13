using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Entities.Borrows;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Common;
using System.Runtime.CompilerServices;
using System.Transactions;
using static CleanCodeLibrary.Domain.Common.Validation.ValidationItems.ValidationItems;
using borrowDto = CleanCodeLibrary.Domain.Entities.Borrows.Borrow;



namespace CleanCodeLibrary.Application.Borrows.Borrow
{
    public class CreateBorrowAndUpdateBookAmountRequest
    {
        public int StudentId { get; set; }
        public int BookId { get; set; }
        public DateTime DueDate { get; set; }
        public int Amount { get; set; } //koliko knjiga zeli
    }
    public class CreateBorrowAndUpdateBookAmountRequestHandler : RequestHandler<CreateBorrowAndUpdateBookAmountRequest, SuccessPostResponse>
    {
        public IBorrowUnitOfWork _unitOfWork;
        private readonly ICacheService<GetAllResponse<BookDto>> _cache;
        public CreateBorrowAndUpdateBookAmountRequestHandler(IBorrowUnitOfWork unitOfWork, ICacheService<GetAllResponse<BookDto>> cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        protected override async Task<Result<SuccessPostResponse>> HandleRequest(CreateBorrowAndUpdateBookAmountRequest request, Result<SuccessPostResponse> result)
        {
            await _unitOfWork.CreateTransaction();

            try
            { 

                var borrowDto = new borrowDto
                {
                    StudentId = request.StudentId,
                    BookId = request.BookId,
                    DueDate = DateOnly.FromDateTime(request.DueDate),
                    AmountBorrowed = request.Amount, 
                };
                var domainResult = await borrowDto.BorrowBook(_unitOfWork);
                result.SetValidationResult(domainResult.ValidationResult);

                if (result.HasError) 
                {
                    await _unitOfWork.Rollback();
                    return result;
                }

                await _unitOfWork.SaveAsync();
                await _unitOfWork.Commit();

                _cache.Invalidate(Keys.AllBooks);
                _cache.Invalidate(Keys.TopBooks10);  

                result.SetResult(new SuccessPostResponse(borrowDto.Id));

                return result;
            }

            catch (Exception ex)
            {
                await _unitOfWork.Rollback();  // ← PONIŠTAVANJE SVE
                return result;
            }

        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
