using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Application.Common.Interfaces;

namespace CleanCodeLibrary.Application.Students.Student
{
    public class GetByIdRequest
    {
        public int Id
        {
            get; set;
        }
    }

    public class GetByIdRequestHandler : RequestHandler<GetByIdRequest, StudentDto>
    {
        private readonly IStudentRepository _studentRepository;

        private readonly ICurrentUserService _currentUser;

        public GetByIdRequestHandler(IStudentRepository studentRepository, ICurrentUserService currentUser)
        {
            _studentRepository = studentRepository;
            _currentUser = currentUser;
        }

        protected async override Task<Result<StudentDto>> HandleRequest(
            GetByIdRequest request,
            Result<StudentDto> result)
        {
            var id = _currentUser.GetStudentId();

            if (id == null)
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Student.InvalidId));

                return result;
            }

            var studentDto = await _studentRepository.GetDtoById(id.Value);

            if (studentDto == null)
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Student.NotFound));

                return result;
            }

            result.SetResult(studentDto);

            return result;
        }

        protected override Task<bool> IsAuthorized()
        {
            if (!_currentUser.IsAuthenticated())
                return Task.FromResult(false);

            var role = _currentUser.GetRole();

            return Task.FromResult(role == "Admin" || role == "Student");
        }
    }
}
