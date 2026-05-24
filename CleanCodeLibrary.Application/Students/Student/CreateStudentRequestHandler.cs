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
    public class CreateStudentRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateOnly? DateOfbirth { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class CreateStudentRequestHandler : RequestHandler<CreateStudentRequest, SuccessPostResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICacheService<GetAllResponse<StudentDto>> _cache;

        public CreateStudentRequestHandler(
            IStudentRepository studentRepository,
            IPasswordHasher passwordHasher,
            ICacheService<GetAllResponse<StudentDto>> cache)
        {
            _studentRepository = studentRepository;
            _passwordHasher = passwordHasher;
            _cache = cache;
        }

        protected override async Task<Result<SuccessPostResponse>> HandleRequest(CreateStudentRequest request, Result<SuccessPostResponse> result)
        {
            var passwordValidation = StudentEntity.ValidatePassword(request.Password);

            result.SetValidationResult(passwordValidation);

            if (result.HasError)
                return result;

            var student = new StudentEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfbirth,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };

            var validationResult = student.Validate();

            if (!validationResult.HasError && !string.IsNullOrWhiteSpace(student.Email))
            {
                var emailTaken = await _studentRepository.IsEmailTaken(student.Email, student.Id);

                if (emailTaken)
                    validationResult.AddValidationItem(ValidationItems.Student.EmailTaken);
            }

            result.SetValidationResult(validationResult);

            if (result.HasError)
                return result;

            await _studentRepository.InsertAsync(student);

            await _studentRepository.SaveAsync();

            _cache.Invalidate(Keys.AllStudents);

            result.SetResult(new SuccessPostResponse(student.Id));

            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
