using CleanCodeLibrary.Application.Common.Model;
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

        public DeleteStudentRequestHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        //SuccessPostResponse uredu? jer svakako bi valjda vratio id studenta koji je obrisan
        protected async override Task<Result<SuccessPostResponse>> HandleRequest(DeleteStudentRequest request, Result<SuccessPostResponse> result) 
        {
            //JE LI BOLJE FIND ODE ILI DIREKTNO U REPOZITORIJU
           
            //    var domainResult = await CleanCodeLibrary.Domain.Entities.Students.Student
            //        .GetByIdDomainAsync(_studentRepository, request.Id);
            //    result.SetValidationResult(domainResult.ValidationResult);

            //    if (result.HasWarning) //nemas sta brisat ako nema nicega
            //        return result; Nema potrebe trazit kad se trazi u repozitoriju u delete metodi

            var domainDeleteResult = await CleanCodeLibrary.Domain.Entities.Students.Student.Delete(_studentRepository, request.Id);
            result.SetValidationResult(domainDeleteResult.ValidationResult);  // staje 

            if (result.HasError) 
            {
                return result;
            }

            await _studentRepository.SaveAsync();
            //nisam instacira znaci moram direktno iz repoziroija zvat savechanges

            result.SetResult(new SuccessPostResponse(domainDeleteResult.Value));
            return result;
        
        }
        protected override Task<bool> IsAuthorized()
        {
            return Task.FromResult(true);
        }
    }
}
