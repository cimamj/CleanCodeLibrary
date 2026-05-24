using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.CacheKeys;

namespace CleanCodeLibrary.Application.Students.Student
{
    public class GetAllStudentsRequest
    {
    }

    public class GetAllStudentsRequestHandler : RequestHandler<GetAllStudentsRequest, GetAllResponse<StudentDto>>
    {
        private readonly IStudentRepository _studentRepository;

        private readonly ICurrentUserService _currentUser;

        private readonly ICacheService<GetAllResponse<StudentDto>> _cache;

        public GetAllStudentsRequestHandler(IStudentRepository studentRepository, ICurrentUserService currentUser, ICacheService<GetAllResponse<StudentDto>> cache)
        {
            _studentRepository = studentRepository;
            _currentUser = currentUser;
            _cache = cache;
        }

        protected async override Task<Result<GetAllResponse<StudentDto>>> HandleRequest(
            GetAllStudentsRequest request,
            Result<GetAllResponse<StudentDto>> result
        )
        {
            var students = await _cache.GetOrSetAsync(Keys.AllStudents, () => _studentRepository.GetAllStudentDtos(), TimeSpan.FromMinutes(7));

            if (students.Values.Count() == 0)
            {
                result.AddWarning(new ValidationResultItem
                {
                    Message = "Nema nijednog studenta u bazi",
                    ValidationSeverity = ValidationSeverity.Warning,
                });
            }

            result.SetResult(students);

            return result;
        }

        protected override async Task<bool> IsAuthorized()
        {
            var studentId = _currentUser.GetStudentId();
            if (studentId == null)
                return false;

            var student = await _studentRepository.GetById(studentId.Value);
            if (student == null)
                return false;
            else if (student.Role != "Admin")
                return false;

            return true;
        }
    }
}
