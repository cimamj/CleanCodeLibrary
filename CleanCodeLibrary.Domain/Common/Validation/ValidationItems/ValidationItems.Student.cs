namespace CleanCodeLibrary.Domain.Common.Validation.ValidationItems
{
    public static partial class ValidationItems
    {
        public static class Student
        {
            public static string CodePrefix = nameof(Student);

            public static readonly ValidationItem FirstNameMaxLength = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Ime ne smije biti duze od {Entities.Students.Student.NameMaxLength} znakova",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem LastNameMaxLength = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Prezime ne smije biti duze od {Entities.Students.Student.NameMaxLength} znakova",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem FirstNameNull = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Ime ne smije biti prazno",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem LastNameNull = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Prezime ne smije biti prazno",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem No_Students = new ValidationItem
            {
                Code = $"NO_STUDENTS",
                Message = $"Nema studenata u bazi",
                ValidationSeverity = ValidationSeverity.Warning,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem No_Student = new ValidationItem
            {
                Code = $"NO_WANTED_STUDENT",
                Message = $"Nema trazenog studenta u bazi",
                ValidationSeverity = ValidationSeverity.Warning,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem DateOfBirthNull = new ValidationItem
            {
                Code = $"{CodePrefix}DateOfBirthNull",
                Message = $"Niste unijeli datum rodenja",
                ValidationSeverity = ValidationSeverity.Warning,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem Future = new ValidationItem
            {
                Code = $"{CodePrefix}DateOfBirthInFuture",
                Message = $"Datum rodenja ne može biti u buducnosti",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem DeleteWentWrong = new ValidationItem
            {
                Code = $"STUDENT_NOT_DELETED",
                Message = $"Student se nije uspio obrisati",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.SystemError
            };

            public static readonly ValidationItem LoginFailed = new ValidationItem
            {
                Code = $"STUDENT_LOGIN_FAILED",
                Message = $"Student s ovom sifrom ne postoji u bazi, kriva sifra ili mail",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.BussinessRule
            };

            public static readonly ValidationItem NotFound = new ValidationItem
            {
                Code = $"STUDENT_NOT_FOUND",
                Message = $"Student ne postoji u bazi",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.NotFound
            };

            public static readonly ValidationItem InvalidId = new ValidationItem
            {
                Code = $"STUDENT_INVALID_ID",
                Message = $"Krivi Id",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem EmailTaken = new ValidationItem
            {
                Code = $"EMAIL_TAKEN",
                Message = $"Student s ovim emailom vec postoji",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.SystemError
            };

            public static readonly ValidationItem EmailNull = new ValidationItem
            {
                Code = $"EMAIL_NULL",
                Message = $"Email ne smije biti prazan",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.SystemError
            };

            public static readonly ValidationItem EmailWrongFormat = new ValidationItem
            {
                Code = $"EMAIL_FORMAT",
                Message = $"Email krivi format, treba koristiti @ ",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.SystemError
            };

            public static readonly ValidationItem PasswordFormat = new ValidationItem
            {
                Code = $"PASSWORD_FORMAT",
                Message = $"Lozinka mora sadržavati barem jedan broj i veliko slovo",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.SystemError
            };

            public static readonly ValidationItem PasswordMinimum = new ValidationItem
            {
                Code = $"PASSWORD_FORMAT",
                Message = $"Lozinka mora imati barem 8 znakova",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.SystemError
            };

            public static readonly ValidationItem No_Active_Borrows = new ValidationItem
            {
                Code = $"NO_ACTIVE_BORROWS",
                Message = $"Nema aktivnih posudbi, vracene su",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };
        }
    }
}
