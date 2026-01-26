using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Entities.Borrows;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Common;
using System.Runtime.CompilerServices;
using System.Transactions;
using static CleanCodeLibrary.Domain.Common.Validation.ValidationItems.ValidationItems;


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
        public IUnitOfWork _unitOfWork;
        public CreateBorrowAndUpdateBookAmountRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override async Task<Result<SuccessPostResponse>> HandleRequest(CreateBorrowAndUpdateBookAmountRequest request, Result<SuccessPostResponse> result)
        {
            await _unitOfWork.CreateTransaction();

            try
            { //dakle dodam redak u borrow, pogleda validaijca dopusta li uopce jer ona ce gledat ima li knjiga vise

                //pokusam stvorit posudbu !!!!
                var borrowDto = new CleanCodeLibrary.Domain.Entities.Borrows.Borrow
                {
                    StudentId = request.StudentId,
                    BookId = request.BookId,
                    DueDate = request.DueDate,
                };
                var domainResult = await borrowDto.BorrowBook(_unitOfWork, request.Amount);
                result.SetValidationResult(domainResult.ValidationResult);

                if (result.HasError) //error ili je nesto null ili nema dovoljno knjiga na lageru za trazeni req.amount
                {
                    //mozda rollback?
                    await _unitOfWork.Rollback();
                    return result;
                }
                //ako nema errora e sad smanji broj knjiga, tj update
                //prvo ce je tribat dohvatiti tu istu, 

                //ako uspije posudba, knjiga na lageru ima idemo oduzet stanje knjizi !!!!!
                var bookResult = await CleanCodeLibrary.Domain.Entities.Books.Book.GetByIdDomainAsync(_unitOfWork.BookRepository, request.BookId);
                result.SetValidationResult(bookResult.ValidationResult);
                if (result.HasError)
                {
                    await _unitOfWork.Rollback();
                    return result;
                }
                var book = bookResult.Value;
                book.Amount -= request.Amount;

                var domainBookResult = await book.Update(_unitOfWork.BookRepository);
                result.SetValidationResult(domainBookResult.ValidationResult);
                if (result.HasError)
                {
                    return result;
                }
                //do ode niusta

                await _unitOfWork.SaveAsync();
                await _unitOfWork.Commit(); 

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
