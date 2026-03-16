using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Persistance.Students;
using System.Reflection.Metadata.Ecma335;
using CleanCodeLibrary.Domain.Entities.Borrows;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Persistance.Borrows; //nije dovoljno samo ovo gori da ukljuci nesto iz foldera ranije
using System.ComponentModel.DataAnnotations.Schema; //za notmapped 
using System.Text.Json.Serialization; //jsonignore

namespace CleanCodeLibrary.Domain.Entities.Students
{
    public class Student
    {
        public const int NameMaxLength = 100;
        public int Id { get; set; }
        public string FirstName { get; set; } //ne more bit null kako to ? =string.Empty;
        public string LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }  //mozda warning ako saljes bez date, prolazi i roden u buducnosti format falidacija

        public ICollection<Borrow> Borrows { get; set; }

        //za autorizaciju
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        //privremeno polje, da validacija bude ode, ne smis ga u konfg dodavat, ne smi ga EF mapirati
        [JsonIgnore] // ne salje se u JSON response
        [NotMapped]  // EF ne sprema ovo u bazu niti cita iz nje
        public string? Password { get; set; } //za update ? mozda nece minjat sifru
        public string Role { get; set; } = "Student"; //Default


        //public void SetPassword(string plainPassword)
        //{
        //    if (string.IsNullOrWhiteSpace(plainPassword))
        //        throw new DomainValidationException("Lozinka je obavezna");

        //    if (plainPassword.Length < 8)
        //        throw new DomainValidationException("Lozinka mora imati barem 8 znakova");

        //    if (!plainPassword.Any(char.IsDigit) || !plainPassword.Any(char.IsUpper))
        //        throw new DomainValidationException("Lozinka mora sadržavati barem jedan broj i veliko slovo");

        //    Password = plainPassword;               // privremeno čuvamo plain text za validaciju
        //    PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword); // hashiramo i spremamo
        //}
        public void SetPassword(string plainPassword)
        {
            Password = plainPassword; 
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }
        public async Task<ResultDomain<int?>> Create(IStudentRepository studentRepository) //tip argumenta interface je iz domaina, a studentRepository iz infrastrucutra, dakle ODVOJENO JE, DOMAIN NE OVISI O NIKOME, ova se metoda poziva iz app, app salje taj infra
        {
            var validationResult = await CreateOrUpdateValidation(studentRepository); //AWAIT DODAJ JER JE ASYNC
            if (validationResult.HasError)
            {
                return new ResultDomain<int?>(null, validationResult); //this  iz app u kojem se mapira dto iznad handlera u ovog studenta, .create od ive ivica
            }
            //PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password); u handleru ili ode?
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
            var validationResult = await CreateOrUpdateValidation(studentRepository); //zasto ode triba kad nema i/o operacija u ovoj metodi
            if (validationResult.HasError) 
            {
                return new ResultDomain<int?>(null, validationResult);
                
            }
            studentRepository.Update(this); //Ne triba await u ovom slucaju jer nije asinkrona ali za savechanges zovemo iz aplicationsloja tj handlera nakon ovog poziva  jel to ok?
            return new ResultDomain<int?>(Id, validationResult);
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

        public async Task<ResultDomain<GetAllResponse<ActiveBorrowsDto>>> ActiveBorrows(IBorrowUnitOfWork unitOfWork)
        {
            var student = await unitOfWork.StudentRepository.GetById(Id);
            var validationResult = new ValidationResult();

            if (student == null)
            {
                validationResult.AddValidationItem(ValidationItems.Student.No_Student);
                return new ResultDomain<GetAllResponse<ActiveBorrowsDto>>(null, validationResult);
            }

            var activeBorrows = await unitOfWork.StudentRepository.GetActiveBorrowsDtos(Id);
            Console.WriteLine(activeBorrows);
            if (!activeBorrows.Values.Any())
            {

                validationResult.AddValidationItem(ValidationItems.Student.No_Active_Borrows);
                return new ResultDomain<GetAllResponse<ActiveBorrowsDto>>(null, validationResult);
            }
            return new ResultDomain<GetAllResponse<ActiveBorrowsDto>>(activeBorrows, validationResult);
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

        public async Task<ValidationResult> CreateOrUpdateValidation(IStudentRepository studentRepository) //Z A S T O je ovo async?! zasto kad puknem static se crvene neki parametri?
        {
            var validationResult = new ValidationResult();
            if (string.IsNullOrWhiteSpace(FirstName))  
                validationResult.AddValidationItem(ValidationItems.Student.FirstNameNull); //ova metoda se koristi preko IF odavde i puni se sa ovom van te klase

            else if(FirstName.Length > NameMaxLength)
                validationResult.AddValidationItem(ValidationItems.Student.FirstNameMaxLength);

            if (string.IsNullOrWhiteSpace(LastName))
                validationResult.AddValidationItem(ValidationItems.Student.LastNameNull);

            else if (LastName.Length > NameMaxLength) //koliko if (uvjeta) toliko static objekata 
                validationResult.AddValidationItem(ValidationItems.Student.LastNameMaxLength);

            if (string.IsNullOrWhiteSpace(Email))
                validationResult.AddValidationItem(ValidationItems.Student.EmailNull);

            else if(!Email.Contains("@"))
                validationResult.AddValidationItem(ValidationItems.Student.EmailWrongFormat);

            else
            {
                var isEmailTaken = await studentRepository.IsEmailUnique(Email, Id);

                if(isEmailTaken)
                    validationResult.AddValidationItem(ValidationItems.Student.EmailTaken);
            }



            if (Password != null) //u update ce biti null, ne moze je povuci iz baze
            {
                if (Password.Length < 8)
                    validationResult.AddValidationItem(ValidationItems.Student.PasswordMinimum);
                else if (!Password.Any(char.IsDigit) || !Password.Any(char.IsUpper))
                    validationResult.AddValidationItem(ValidationItems.Student.PasswordFormat);
            }



            if (!DateOfBirth.HasValue)
                validationResult.AddValidationItem(ValidationItems.Student.DateOfBirthNull);
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
