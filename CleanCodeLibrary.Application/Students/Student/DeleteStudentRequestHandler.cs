using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Persistance.Students;

namespace CleanCodeLibrary.Application.Students.Student
{
    public class DeleteStudentRequest
    {
        public int Id { get; set; }
    }

    public class DeleteStudentRequestHandler : RequestHandler<DeleteStudentRequest, SuccessPostResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICacheService<GetAllResponse<StudentDto>> _cache;

        public DeleteStudentRequestHandler(
            IStudentRepository studentRepository,
            ICacheService<GetAllResponse<StudentDto>> cache)
        {
            _studentRepository = studentRepository;
            _cache = cache;
        }

        protected override async Task<Result<SuccessPostResponse>> HandleRequest(DeleteStudentRequest request, Result<SuccessPostResponse> result)
        {
            var studentExists = await _studentRepository.GetById(request.Id);

            if (studentExists == null)
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Student.NotFound));

                return result;
            }

            var deleted = await _studentRepository.DeleteAsync(request.Id);

            if (!deleted)
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Student.DeleteWentWrong));

                return result;
            }

            await _studentRepository.SaveAsync();

            _cache.Invalidate(Keys.AllStudents);

            result.SetResult(new SuccessPostResponse(request.Id));

            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
