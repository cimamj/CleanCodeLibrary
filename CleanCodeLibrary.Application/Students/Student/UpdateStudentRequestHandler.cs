using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Persistance.Students;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;


namespace CleanCodeLibrary.Application.Students.Student
{
    public class UpdateStudentRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
    public class UpdateStudentRequestHandler : RequestHandler<UpdateStudentRequest, SuccessPostResponse>
    {
        private readonly IStudentRepository _studentRepository;

        public UpdateStudentRequestHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        protected async override Task<Result<SuccessPostResponse>> HandleRequest(UpdateStudentRequest request, Result<SuccessPostResponse> result) //ne  zaboravi dodat Result<> jer nemos pritupit validaiciji, btw kad se ovo instacira pa da ga korsiitm
        {

            //dohvati postojeceg kojeg minjas valjda
            //ovo preko domaina nemoj ovako dirketno
            //prvo vidi ima li error u smislu  da uopce nije dohvatio studenta vidis to iz validacije
            //var existingStudent = new CleanCodeLibrary.Domain.Entities.Students.Student();
            //CleanCodeLibrary.Domain.Entities.Students.Student.GetByIdDomainAsync(_studentRepository, request.Id);
            //var domainResult = await CleanCodeLibrary.Domain.Entities.Students.Student
            //    .GetByIdDomainAsync(_studentRepository, request.Id);
            var studentRepoResult = await _studentRepository.GetById(request.Id);
            if (studentRepoResult == null)
            {
                result.AddError(new ValidationResultItem
                {
                    Code = "Student.NotFound",
                    Message = "Student ne postoji u bazi",
                    ValidationSeverity = ValidationSeverity.Error,
                    ValidationType = ValidationType.NotFound //novo dodano, rjesi za create
                });
                return result;

            }



            //ako je dohvatio onda promijeni,ali prvojeri validationResult da nije ime predugo itd...
            //Jel ovo dobro ili tribalo instacirati??
            studentRepoResult.FirstName = request.FirstName;
            studentRepoResult.LastName = request.LastName;
            studentRepoResult.DateOfBirth = request.DateOfBirth; 

            var validationResult = await studentRepoResult.Update(_studentRepository);
           
            result.SetValidationResult(validationResult.ValidationResult); //jel se sad isti Result promjenio svoj ValidationResult?
            if (result.HasError) //dugo ime je error
                return result;

            await studentRepoResult.SaveChanges(_studentRepository); //komuniciramo s domain ne infra

            result.SetResult(new SuccessPostResponse(studentRepoResult.Id)); //koji id jel ovi ili novi od req
            return result;

        }

        protected override Task<bool> IsAuthorized() 
        {
            return Task.FromResult(true);   
        } 
    }
}
