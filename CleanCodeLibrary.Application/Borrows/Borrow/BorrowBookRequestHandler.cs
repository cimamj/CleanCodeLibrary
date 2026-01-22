using CleanCodeLibrary.Application.Common.Model;
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
    }
    public class BorrowBookRequestHandler : RequestHandler<BorrowBookRequest, SuccessPostResponse>
    {
        public IUnitOfWork _unitOfWork;
        public BorrowBookRequestHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        protected override async Task<Result<SuccessPostResponse>> HandleRequest(BorrowBookRequest request, Result<SuccessPostResponse> result)
        {
            var borrowDto = new CleanCodeLibrary.Domain.Entities.Borrows.Borrow
            {
                StudentId = request.StudentId,
                BookId = request.BookId,
                DueDate = request.DueDate,
            };
            var domainResult = await borrowDto.BorrowBook(_unitOfWork);

            result.SetValidationResult(domainResult.ValidationResult);

            if (result.HasError)
            {
                //mozda rollback?
                await _unitOfWork.Rollback();
                return result;
            }

            await _unitOfWork.SaveAsync();
            await _unitOfWork.Commit(); //jel triba ovo 

            result.SetResult(new SuccessPostResponse(domainResult.Value)); 

            return result;

        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
