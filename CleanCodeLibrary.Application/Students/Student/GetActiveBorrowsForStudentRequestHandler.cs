using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.Common.Model;
using System.Linq;

namespace CleanCodeLibrary.Application.Students.Student
{
    public class GetActiveBorrowsRequest
    {
        public int Id
        {
            get; set;
        }
    }

    public class GetActiveBorrowsForStudentRequestHandler : RequestHandler<GetActiveBorrowsRequest, GetAllResponse<ActiveBorrowsDto>>
    {
        private readonly IStudentRepository _studentRepository;

        public GetActiveBorrowsForStudentRequestHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        protected async override Task<Result<GetAllResponse<ActiveBorrowsDto>>> HandleRequest(
            GetActiveBorrowsRequest request,
            Result<GetAllResponse<ActiveBorrowsDto>> result)
        {
            var activeBorrows = await _studentRepository.GetActiveBorrowsDtos(request.Id);

            if (activeBorrows.Values == null || !activeBorrows.Values.Any())
            {
                result.AddWarning(new ValidationResultItem
                {
                    Message = "Nema aktivnih posudbi za ovog studenta.",
                    ValidationSeverity = ValidationSeverity.Warning,
                });
            }

            result.SetResult(activeBorrows);

            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
