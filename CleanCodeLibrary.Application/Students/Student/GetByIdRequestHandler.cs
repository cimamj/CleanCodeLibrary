using CleanCodeLibrary.Application.Common.Model;  // ← OVO JE KLJUČNO – koristi tvoju Result iz Application sloja!
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Application.Common.Interfaces;

namespace CleanCodeLibrary.Application.Students.Student
{
    public class GetByIdRequest
    {
        public int Id { get; set; }
    }

    //public class GetByIdResponse
    //{
    //    public StudentDto Student { get; set; }

    //    public GetByIdResponse(StudentDto student)  // ← mali 's' u parametru
    //    {
    //        Student = student;  // ← koristi parametar
    //    }

    //    public GetByIdResponse() { }
    //}

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
                result.AddError(new ValidationResultItem
                {
                    Code = "Student.WrongId",
                    Message = "Krivi Id",
                    ValidationSeverity = ValidationSeverity.Error,
                    ValidationType = ValidationType.NotFound
                });
                return result;
            }
                
            var studentDto = await _studentRepository.GetDtoById(id.Value);
            
            if(studentDto == null)
            {
                result.AddError(new ValidationResultItem
                {
                    Code = "Student.NotFound",
                    Message = "Student ne postoji",
                    ValidationSeverity = ValidationSeverity.Error,
                    ValidationType = ValidationType.NotFound
                });
                return result;
            }

            result.SetResult(studentDto);
            return result;
        }

        protected override Task<bool> IsAuthorized()
        {
            if (!_currentUser.IsAuthenticated())
                return Task.FromResult(false); //middleware odradi, ali edge case eto 

            var role = _currentUser.GetRole();
            return Task.FromResult(role == "Admin" || role == "Student");
            //return Task.FromResult(_currentUser.IsAdmin());
        }
    }
}