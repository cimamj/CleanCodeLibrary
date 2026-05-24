using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Persistance.Students;
using StudentEntity = CleanCodeLibrary.Domain.Entities.Students.Student;

namespace CleanCodeLibrary.Application.Students.Student
{
    public class UpdateStudentRequest
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? NewPassword { get; set; }
    }

    public class UpdateStudentRequestHandler : RequestHandler<UpdateStudentRequest, SuccessPostResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICacheService<GetAllResponse<StudentDto>> _cache;

        public UpdateStudentRequestHandler(
            IStudentRepository studentRepository,
            ICurrentUserService currentUser,
            IPasswordHasher passwordHasher,
            ICacheService<GetAllResponse<StudentDto>> cache)
        {
            _studentRepository = studentRepository;
            _currentUser = currentUser;
            _passwordHasher = passwordHasher;
            _cache = cache;
        }

        protected override async Task<Result<SuccessPostResponse>> HandleRequest(UpdateStudentRequest request, Result<SuccessPostResponse> result)
        {
            var role = _currentUser.GetRole();

            int targetId = role == "Student" ? _currentUser.GetStudentId().Value : request.Id;

            var existingStudent = await _studentRepository.GetById(targetId);

            if (existingStudent == null)
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Student.NotFound));

                return result;
            }

            if (!string.IsNullOrWhiteSpace(request.FirstName))
                existingStudent.FirstName = request.FirstName;

            if (!string.IsNullOrWhiteSpace(request.LastName))
                existingStudent.LastName = request.LastName;

            if (request.DateOfBirth.HasValue)
                existingStudent.DateOfBirth = request.DateOfBirth;

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                var passwordValidation = StudentEntity.ValidatePassword(request.NewPassword);

                result.SetValidationResult(passwordValidation);

                if (result.HasError)
                    return result;

                existingStudent.PasswordHash = _passwordHasher.Hash(request.NewPassword);
            }

            var validationResult = existingStudent.Validate();

            if (!validationResult.HasError && !string.IsNullOrWhiteSpace(existingStudent.Email))
            {
                var emailTaken = await _studentRepository.IsEmailTaken(existingStudent.Email, existingStudent.Id);

                if (emailTaken)
                    validationResult.AddValidationItem(ValidationItems.Student.EmailTaken);
            }

            result.SetValidationResult(validationResult);

            if (result.HasError)
                return result;

            _studentRepository.Update(existingStudent);

            await _studentRepository.SaveAsync();

            _cache.Invalidate(Keys.AllStudents);

            result.SetResult(new SuccessPostResponse(existingStudent.Id));

            return result;
        }

        protected override Task<bool> IsAuthorized()
        {
            var role = _currentUser.GetRole();

            return Task.FromResult(role == "Admin" || role == "Student");
        }
    }
}
