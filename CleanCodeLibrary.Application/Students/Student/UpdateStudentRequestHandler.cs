using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Persistance.Students;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;


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
        private readonly ICacheService<GetAllResponse<StudentDto>> _cache;

        public UpdateStudentRequestHandler(IStudentRepository studentRepository, ICurrentUserService currentUser, ICacheService<GetAllResponse<StudentDto>> cache)
        {
            _studentRepository = studentRepository;
            _currentUser = currentUser;
            _cache = cache;
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

            var role = _currentUser.GetRole();

            int currId;
            if (role == "Student")
                currId = _currentUser.GetStudentId().Value; // iz tokena, student more sebe samo
            else
                currId = request.Id; //iz bodya/req, ADMIN sve more



            var studentRepoResult = await _studentRepository.GetById(currId);
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
            if (!string.IsNullOrWhiteSpace(request.FirstName))
                studentRepoResult.FirstName = request.FirstName;

            if (!string.IsNullOrWhiteSpace(request.LastName))
                studentRepoResult.LastName = request.LastName;

            if (request.DateOfBirth.HasValue)
                studentRepoResult.DateOfBirth = request.DateOfBirth;

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
                studentRepoResult.SetPassword(request.NewPassword);

            var validationResult = await studentRepoResult.Update(_studentRepository);
           
            result.SetValidationResult(validationResult.ValidationResult); //jel se sad isti Result promjenio svoj ValidationResult?
            if (result.HasError) //dugo ime je error
                return result;

            await studentRepoResult.SaveChanges(_studentRepository); //komuniciramo s domain ne infra
            _cache.Invalidate(Keys.AllStudents);

            result.SetResult(new SuccessPostResponse(studentRepoResult.Id)); //koji id jel ovi ili novi od req
            return result;

        }

        protected override Task<bool> IsAuthorized()  //ode se analiziraju uloge , da pustimo studenta i admina
        { //admin sve, user samo sebe moze update
            //var currentId = _currentUser.GetStudentId();
            //if (currentId == null)
            //    return Task.FromResult(false);

            var role = _currentUser.GetRole();
            return Task.FromResult(role == "Admin" || role == "Student");
        } //Middleware već garantira da je token valjan pa null provjera nije potrebna.
    }
}


//protected override Task<bool> IsAuthorized()
//{
//    var role = _currentUser.GetRole();
//    var currentId = _currentUser.GetStudentId();

//    // Admin može sve
//    if (role == "Admin") return Task.FromResult(true);

//    // Ako je Student, smije samo ako je ID iz tokena isti kao ID u zahtjevu (requestu)
//    return Task.FromResult(role == "Student" && currentId == request.Id);
//}