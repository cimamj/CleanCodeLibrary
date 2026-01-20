
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Students;

namespace CleanCodeLibrary.Application.Students.Student
{
    public class CreateStudentRequest //dto koji primas presentation layera, vraca dto successpostresponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? DateOfbirth { get; set; }
        //dto, npr nema pristup oibu
    }
    public class CreateStudentRequestHandler : RequestHandler<CreateStudentRequest, SuccessPostResponse>
    {
        //saljemo u create Repo , di ti je??
        private readonly IStudentRepository _studentRepository; //interface iz domaina, iako je ovaj objekt iz infrastrukture, tj referenca, dependency injection? nije objekt nego referencana ovaj interface,on ima metodu saveasync, 

        public CreateStudentRequestHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        protected async override Task<Result<SuccessPostResponse>> HandleRequest(CreateStudentRequest request, Result<SuccessPostResponse> result) //protected vidljivo samo ode i child, zasto?????
        {
            var student = new CleanCodeLibrary.Domain.Entities.Students.Student //ode je mapiranje, ne u app
            {
                FirstName = request.FirstName, 
                LastName = request.LastName,
                DateOfBirth = request.DateOfbirth
            };

            var validationResult = await student.Create(_studentRepository); //create izvrsava validaciju (posl log) i dodavanje u ef memoriju (insert)
            result.SetValidationResult(validationResult.ValidationResult); //jer mi dobijemo klasu result koja ima polje value (this id) i validationresult, ode se odmah razvrstava u errore info warning liste

            if(result.HasError) //ne validationResult sta je u ive 
            {
                return result; //success... ima i prazni konstruktor mozda onda moze value bit prazan? value iz resulta je klasa i moze bit null, DA VALUE CE OAVKO OSTAT PRAZAN, NISI GA POPUNIA BIT CE NULL, API će dobiti: { "value": null, "errors": [...] }
            }

            await student.SaveChanges(_studentRepository); //vec je pozvano u insertu // u domainu je InsertAsync time se dodaje u ef memoriju, ode je Save salje se sql insert u bazu
            //mozda i nije potrebno jer vec insert ima u sebi saveasync 
            //ili da maknem iz repozitorija svake metode save i zovem samo iz unitofwork

            result.SetResult(new SuccessPostResponse(student.Id)); //tek sad onaj value iz Result postaje taj id
            return result;

            //ovaj resutl je referneced type, dakle ovim modificiranjem tj punjenjem ce se vratiti to sto zelis, cime si ga napunio iz parent klase

        }

        protected override Task<bool> IsAuthorized() //sve abstraktne impementiraj prvo
        {
            return Task.FromResult(true);   //valjda ovak
        } //Po defaultu result.isAuthorized = true – da, ali ova metoda može to prebrisati na false ako treba.
    }
}
