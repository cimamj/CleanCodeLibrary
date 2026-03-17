

using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Persistance.Borrows;

namespace CleanCodeLibrary.Application.Borrows.Borrow
{
    public record GetBorrowStatisticsRequest(int StudentId);
    public class GetBorrowStatisticsRequestHandler : RequestHandler<GetBorrowStatisticsRequest, BorrowStatisticsDto>
    {
        private readonly IBorrowRepository _borrowRepository;

        public GetBorrowStatisticsRequestHandler(IBorrowRepository borrowRepository)
        {
            _borrowRepository = borrowRepository;
        }

        protected override async Task<Result<BorrowStatisticsDto>> HandleRequest(
            GetBorrowStatisticsRequest request,
            Result<BorrowStatisticsDto> result)
        {
            var stats = await _borrowRepository.GetBorrowStatisticsDtoForStudentAsync(request.StudentId);

            if (stats == null)
            {
                result.AddWarning(new ValidationResultItem
                {
                    Message = "Nema posudbi za ovog studenta.",
                    ValidationSeverity = ValidationSeverity.Error
                });
            }

            result.SetResult(stats ?? new BorrowStatisticsDto());
            return result;
        }

        protected override Task<bool> IsAuthorized()
        {
            return Task.FromResult(true);
        }
    }
}
