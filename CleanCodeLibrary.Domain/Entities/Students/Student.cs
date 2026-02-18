using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Persistance.Students;
using System.Reflection.Metadata.Ecma335; //nije dovoljno samo ovo gori da ukljuci nesto iz foldera ranije

namespace CleanCodeLibrary.Domain.Entities.Students
{
    public class Student
    {
        public const int NameMaxLength = 100;
        public int Id { get; set; }
        public string FirstName { get; set; } //ne more bit null kako to ? =string.Empty;
        public string LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }  //mozda warning ako saljes bez date, prolazi i roden u buducnosti format falidacija

        public async Task<ResultDomain<int?>> Create(IStudentRepository studentRepository) //tip argumenta interface je iz domaina, a studentRepository iz infrastrucutra, dakle ODVOJENO JE, DOMAIN NE OVISI O NIKOME, ova se metoda poziva iz app, app salje taj infra
        {
            var validationResult = await CreateOrUpdateValidation(); //AWAIT DODAJ JER JE ASYNC
            if (validationResult.HasError)
            {
                return new ResultDomain<int?>(null, validationResult); //this  iz app u kojem se mapira dto iznad handlera u ovog studenta, .create od ive ivica
            }
            //ako nema errora, insertaj u bazu this (instanca ovog studenta) i returnaj success 
            await studentRepository.InsertAsync(this); 
            return new ResultDomain<int?>(this.Id, validationResult); //ako prode insertAsync, salji visem sloju appu true
        }

        public static async Task<ResultDomain<Student>> GetByIdDomainAsync(IStudentRepository studentRepository, int id)
        {
            var existingStudent = await studentRepository.GetById(id);
            var validationResult = new ValidationResult();
            if (existingStudent == null)
            {
                validationResult.AddValidationItem(ValidationItems.Student.No_Student);
            }
            return new ResultDomain<Student?>(existingStudent, validationResult);
        }
        public async Task<ResultDomain<int?>> Update(IStudentRepository studentRepository)
        {
            var validationResult = await CreateOrUpdateValidation(); //zasto ode triba kad nema i/o operacija u ovoj metodi
            if (validationResult.HasError) 
            {
                return new ResultDomain<int?>(null, validationResult);
                
            }
            studentRepository.Update(this); //Ne triba await u ovom slucaju jer nije asinkrona ali za savechanges zovemo iz aplicationsloja tj handlera nakon ovog poziva  jel to ok?
            return new ResultDomain<int?>(this.Id, validationResult);
        }

        public static async Task<ResultDomain<GetAllResponse<Student>>> GetAllStudentsAsync(IStudentRepository studentRepository)
        {
            var allStudents = await studentRepository.Get();

            var validationResult = new ValidationResult(); //saljemo praznu
            if(allStudents == null || allStudents.Values == null || !allStudents.Values.Any()) //glupo mi je zvat createorupdate, ako je null ili ako je prazna 
            {
                validationResult.AddValidationItem(ValidationItems.Student.No_Students); //ili da dodam return null umjesto allstudents
                //jel triban return (null, validationResult) ili ce svakako bit null
            }
            return new ResultDomain<GetAllResponse<Student>>(allStudents, validationResult);

        }
        
        public async Task<ResultDomain<int?>> Delete(IStudentRepository studentRepository) //NULLABLE REFERENCE JESE STA TRIBA DIRAT U RESULT KLASI
        {

            var deleteResult = await studentRepository.DeleteAsync(Id);
            var validationResult = new ValidationResult(); //neki livi razlog

            if (!deleteResult) //iz xy razloga nez kojeg mozda novu validaciju dodati za brisanje ode
            {
                validationResult.AddValidationItem(ValidationItems.Student.DeleteWentWrong); //Nije pronaden
                return new ResultDomain<int?>(null, validationResult); //Jel triban return null ili ce se svakako return null
            }
            return new ResultDomain<int?>(Id, validationResult);

        }

        public static async Task<ResultDomain<Student>> GetByIdDomain(IStudentRepository studentRepository, int id)
        {
            var studentById = await studentRepository.GetById(id);
            var validationResult = new ValidationResult();
            if(studentById == null)
            {
                validationResult.AddValidationItem(ValidationItems.Student.No_Student);
                return new ResultDomain<Student?>(null, validationResult);
            }

            return new ResultDomain<Student>(studentById, validationResult);
        }
        public async Task SaveChanges(IStudentRepository studentRepository)
        {
            await studentRepository.SaveAsync();
        }

        public async Task<ValidationResult> CreateOrUpdateValidation() //Z A S T O je ovo async?! zasto kad puknem static se crvene neki parametri?
        {
            var validationResult = new ValidationResult();
            if (string.IsNullOrWhiteSpace(FirstName)) //da nema praznih znakova, i nije null, 
            {
                validationResult.AddValidationItem(ValidationItems.Student.FirstNameNull); //ova metoda se koristi preko IF odavde i puni se sa ovom van te klase
            }
            else if(FirstName.Length > NameMaxLength)
            {
                validationResult.AddValidationItem(ValidationItems.Student.FirstNameMaxLength);
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                validationResult.AddValidationItem(ValidationItems.Student.LastNameNull);
            } //ne more bit null ni u bazi

            else if (LastName.Length > NameMaxLength) //koliko if (uvjeta) toliko static objekata 
            { 
                validationResult.AddValidationItem(ValidationItems.Student.LastNameMaxLength);
            }
            //ako ne prode if, sto ovo vraca? nema nista unutra, U CREATE CES PROVJERITI ELSE KOJI NISI VAMO!!
            if(!DateOfBirth.HasValue)
            {
                validationResult.AddValidationItem(ValidationItems.Student.DateOfBirthNull);
            }
            else
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var birthDate = DateOfBirth.Value;

                if (birthDate > today)
                {
                    validationResult.AddValidationItem(ValidationItems.Student.Future);
                }
            }


            return validationResult;
        }
    }
}
