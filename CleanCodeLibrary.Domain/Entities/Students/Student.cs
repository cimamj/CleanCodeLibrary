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
        public DateOnly? DateOfBirth { get; set; } 

        public async Task<Result<int?>> Create(IStudentRepository studentRepository) //tip argumenta interface je iz domaina, a studentRepository iz infrastrucutra, dakle ODVOJENO JE, DOMAIN NE OVISI O NIKOME, ova se metoda poziva iz app, app salje taj infra
        {
            var validationResult = await CreateOrUpdateValidation(); //AWAIT DODAJ JER JE ASYNC
            if (validationResult.HasError)
            {
                return new Result<int?>(this.Id, validationResult); //this  iz app u kojem se mapira dto iznad handlera u ovog studenta, .create od ive ivica
            }
            //ako nema errora, insertaj u bazu this (instanca ovog studenta) i returnaj success 
            await studentRepository.InsertAsync(this); 
            return new Result<int?>(this.Id, validationResult); //ako prode insertAsync, salji visem sloju appu true
        }

        public static async Task<Result<Student>> GetByIdDomainAsync(IStudentRepository studentRepository, int id)
        {
            var existingStudent = await studentRepository.GetById(id);
            var validationResult = new ValidationResult();
            if (existingStudent == null)
            {
                validationResult.AddValidationItem(ValidationItems.Student.No_Student);
            }
            return new Result<Student?>(existingStudent, validationResult);
        }
        public async Task<Result<int?>> Update(IStudentRepository studentRepository)
        {
            var validationResult = await CreateOrUpdateValidation(); //zasto ode triba kad nema i/o operacija u ovoj metodi
            if (validationResult.HasError) 
            {
                return new Result<int?>(null, validationResult);
                
            }
            studentRepository.Update(this); //Ne triba await u ovom slucaju jer nije asinkrona ali za savechanges zovemo iz aplicationsloja tj handlera nakon ovog poziva  jel to ok?
            return new Result<int?>(this.Id, validationResult);
        }

        public static async Task<Result<GetAllResponse<Student>>> GetAllStudentsAsync(IStudentRepository studentRepository)
        {
            var allStudents = await studentRepository.Get();

            var validationResult = new ValidationResult(); //saljemo praznu
            if(allStudents == null || allStudents.Values == null || !allStudents.Values.Any()) //glupo mi je zvat createorupdate, ako je null ili ako je prazna 
            {
                validationResult.AddValidationItem(ValidationItems.Student.No_Students); //ili da dodam return null umjesto allstudents
                //jel triban return (null, validationResult) ili ce svakako bit null
            }
            return new Result<GetAllResponse<Student>>(allStudents, validationResult);

        }
        
        public static async Task<Result<int?>> Delete(IStudentRepository studentRepository, int id) //NULLABLE REFERENCE JESE STA TRIBA DIRAT U RESULT KLASI
        {

            var deleteResult = await studentRepository.DeleteAsync(id);
            var validationResult = new ValidationResult();

            if (!deleteResult)
            {
                validationResult.AddValidationItem(ValidationItems.Student.No_Student); //Nije pronaden
                return new Result<int?>(null, validationResult); //Jel triban return null ili ce se svakako return null
            }
            return new Result<int?>(id, validationResult);

        }

        public static async Task<Result<Student>> GetByIdDomain(IStudentRepository studentRepository, int id)
        {
            var studentById = await studentRepository.GetById(id);
            var validationResult = new ValidationResult();
            if(studentById == null)
            {
                validationResult.AddValidationItem(ValidationItems.Student.No_Student);
                return new Result<Student?>(null, validationResult);
            }

            return new Result<Student>(studentById, validationResult);
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
            return validationResult; //ako ne prode if, sto ovo vraca? nema nista unutra, U CREATE CES PROVJERITI ELSE KOJI NISI VAMO!!
        }
    }
}
