using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.Common.Model;

namespace CleanCodeLibrary.Application.Students.Student
{
    public class GetActiveBorrowsRequest
    {
        public int Id { get; set; }
    }
    public class GetActiveBorrowsForStudentRequestHandler : RequestHandler<GetActiveBorrowsRequest, GetAllResponse<ActiveBorrowsDto>>
    {
        private readonly IBorrowUnitOfWork _unitOfWork;

        public GetActiveBorrowsForStudentRequestHandler(IBorrowUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async override Task<Result<GetAllResponse<ActiveBorrowsDto>>> HandleRequest(
            GetActiveBorrowsRequest request,
            Result<GetAllResponse<ActiveBorrowsDto>> result)
        {
            var item = new Domain.Entities.Students.Student
            {
                Id = request.Id,
            };

            var domainResult = await item.ActiveBorrows(_unitOfWork);
            var validationResult = domainResult.ValidationResult;

            if (validationResult.HasError)
                return result;
            
            result.SetResult(domainResult.Value);
            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
