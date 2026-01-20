using CleanCodeLibrary.Application.Common.Model;  // ← OVO JE KLJUČNO – koristi tvoju Result iz Application sloja!
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.Entities.Students;

namespace CleanCodeLibrary.Application.Students.Student
{
    public class GetByIdRequest
    {
        public int Id { get; set; }
    }

    public class GetByIdResponse
    {
        public StudentDto Student { get; set; }

        public GetByIdResponse(StudentDto student)  // ← mali 's' u parametru
        {
            Student = student;  // ← koristi parametar
        }

        public GetByIdResponse() { }
    }

    public class GetByIdRequestHandler : RequestHandler<GetByIdRequest, GetByIdResponse>
    {
        private readonly IStudentRepository _studentRepository;

        public GetByIdRequestHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        protected async override Task<Result<GetByIdResponse>> HandleRequest(
            GetByIdRequest request,
            Result<GetByIdResponse> result)
        {
            var domainResult = await CleanCodeLibrary.Domain.Entities.Students.Student.GetByIdDomain(_studentRepository, request.Id);

            result.SetValidationResult(domainResult.ValidationResult);

            if (result.HasError || domainResult.Value == null)
            {
                return result;
            }

            var studentDto = new StudentDto
            {
                Id = domainResult.Value.Id,
                FirstName = domainResult.Value.FirstName,
                LastName = domainResult.Value.LastName,
                DateOfBirth = domainResult.Value.DateOfBirth
            };

            result.SetResult(new GetByIdResponse(studentDto));
            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}