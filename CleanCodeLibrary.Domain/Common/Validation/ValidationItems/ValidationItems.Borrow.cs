//sto je name of borrow jel doslovno borrow

namespace CleanCodeLibrary.Domain.Common.Validation.ValidationItems
{
    public static partial class ValidationItems
    {
        public static class Borrow
        {
            public static string CodePrefix = nameof(Borrow);
            //doslovno polje ne metoda!!!!!
            public static readonly ValidationItem NoBookFound = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Knjiga nije pronadena", 
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem NoStudentFound = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Student nije pronaden",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };
            public static readonly ValidationItem BookBorrowed = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Knjiga nije dostupna",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

        
        }
    }
}
