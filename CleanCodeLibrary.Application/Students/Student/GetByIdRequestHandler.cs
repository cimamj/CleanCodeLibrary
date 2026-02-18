using CleanCodeLibrary.Application.Common.Model;  // ← OVO JE KLJUČNO – koristi tvoju Result iz Application sloja!
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Common.Validation;

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

        public GetByIdRequestHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        protected async override Task<Result<StudentDto>> HandleRequest( //jel triba StudentDto? dodavati svugdje?
            GetByIdRequest request,
            Result<StudentDto> result)
        { 
        //{
        //    var domainResult = await CleanCodeLibrary.Domain.Entities.Students.Student.GetByIdDomain(_studentRepository, request.Id);
        //necemo priko domaina nego direktno

            var studentDto = await _studentRepository.GetDtoById(request.Id);
            
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

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}