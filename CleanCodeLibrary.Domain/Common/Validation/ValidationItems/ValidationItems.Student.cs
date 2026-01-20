

namespace CleanCodeLibrary.Domain.Common.Validation.ValidationItems
{
    public static partial class ValidationItems
    {
        public static class Student
        {
            public static string CodePrefix = nameof(Student);
            //ode se puni validationItem, readonly static ne minja se i 1 instaca u memoriji ista za sve firstnamemaxlenth

            public static readonly ValidationItem FirstNameMaxLength = new ValidationItem //ode se nalaze gotovi objekti validationitema
            {
                Code = $"{CodePrefix}1",
                Message = $"Ime ne smije biti duze od {Entities.Students.Student.NameMaxLength} znakova",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem LastNameMaxLength = new ValidationItem //drugi item, druga validacijska greska
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
                Code = $"{CodePrefix}1", //ovo uvik isto
                Message = $"Prezime ne smije biti prazno",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem No_Students = new ValidationItem
            {
                Code = $"NO_STUDENTS",
                Message = $"Nema studenata u bazi",
                ValidationSeverity = ValidationSeverity.Warning, //ili error
                ValidationType = ValidationType.FormalValidation
            };
            public static readonly ValidationItem No_Student = new ValidationItem
            {
                Code = $"NO_WANTED_STUDENT",
                Message = $"Nema trazenog studenta u bazi",
                ValidationSeverity = ValidationSeverity.Warning, //ili error
                ValidationType = ValidationType.FormalValidation
            };
            //public static readonly ValidationItem Not_Deleted = new ValidationItem
            //{
            //    Code = $"NOT_DELETED_STUDENT",
            //    Message = $"Student nije obrisan,nema trazenog studenta u bazi",
            //    ValidationSeverity = ValidationSeverity.Warning, //ili error
            //    ValidationType = ValidationType.FormalValidation
            //};



        }
    }
}
