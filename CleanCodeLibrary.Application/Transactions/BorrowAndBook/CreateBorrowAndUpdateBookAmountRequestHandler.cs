using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using Microsoft.Extensions.Logging;
using BorrowEntity = CleanCodeLibrary.Domain.Entities.Borrows.Borrow;

namespace CleanCodeLibrary.Application.Borrows.Borrow
{
    public class CreateBorrowAndUpdateBookAmountRequest
    {
        public int StudentId { get; set; }

        public int BookId { get; set; }

        public DateTime DueDate { get; set; }

        public int Amount { get; set; }
    }

    public class CreateBorrowAndUpdateBookAmountRequestHandler : RequestHandler<CreateBorrowAndUpdateBookAmountRequest, SuccessPostResponse>
    {
        private readonly IBorrowUnitOfWork _unitOfWork;
        private readonly ICacheService<GetAllResponse<BookDto>> _cache;
        private readonly ILogger<CreateBorrowAndUpdateBookAmountRequestHandler> _logger;

        public CreateBorrowAndUpdateBookAmountRequestHandler(
            IBorrowUnitOfWork unitOfWork,
            ICacheService<GetAllResponse<BookDto>> cache,
            ILogger<CreateBorrowAndUpdateBookAmountRequestHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }

        protected override async Task<Result<SuccessPostResponse>> HandleRequest(CreateBorrowAndUpdateBookAmountRequest request, Result<SuccessPostResponse> result)
        {
            await _unitOfWork.CreateTransaction();

            try
            {
                var book = await _unitOfWork.BookRepository.GetByIdEntity(request.BookId);

                if (book == null)
                {
                    result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Book.NotFound));

                    await _unitOfWork.Rollback();

                    return result;
                }

                if (book.Amount < request.Amount)
                {
                    result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Borrow.NotEnoughBooks(book.Amount, request.Amount)));

                    await _unitOfWork.Rollback();

                    return result;
                }

                var studentExists = await _unitOfWork.StudentRepository.GetById(request.StudentId) != null;

                if (!studentExists)
                {
                    result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Borrow.NoStudentFound));

                    await _unitOfWork.Rollback();

                    return result;
                }

                var borrow = new BorrowEntity
                {
                    StudentId = request.StudentId,
                    BookId = request.BookId,
                    DueDate = DateOnly.FromDateTime(request.DueDate),
                    AmountBorrowed = request.Amount
                };

                await _unitOfWork.BorrowRepository.InsertAsync(borrow);

                await _unitOfWork.BookRepository.DecrementAmount(request.BookId, request.Amount);

                await _unitOfWork.BookRepository.IncrementBorrowCount(request.BookId, request.Amount);

                await _unitOfWork.SaveAsync();

                await _unitOfWork.Commit();

                _cache.Invalidate(Keys.AllBooks);

                _cache.Invalidate(Keys.TopBooks10);

                result.SetResult(new SuccessPostResponse(borrow.Id));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create borrow for BookId={BookId}, StudentId={StudentId}", request.BookId, request.StudentId);

                await _unitOfWork.Rollback();

                result.AddError(new ValidationResultItem
                {
                    Message = "Doslo je do greske pri posudbivanju knjige.",
                    ValidationSeverity = ValidationSeverity.Error
                });

                return result;
            }
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
