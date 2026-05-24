using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Entities.Borrows;

namespace CleanCodeLibrary.Domain.Entities.Students
{
    public class Student
    {
        public const int NameMaxLength = 100;

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public ICollection<Borrow> Borrows { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; } = "Student";

        public ValidationResult Validate()
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrWhiteSpace(FirstName))
                validationResult.AddValidationItem(ValidationItems.Student.FirstNameNull);
            else if (FirstName.Length > NameMaxLength)
                validationResult.AddValidationItem(ValidationItems.Student.FirstNameMaxLength);

            if (string.IsNullOrWhiteSpace(LastName))
                validationResult.AddValidationItem(ValidationItems.Student.LastNameNull);
            else if (LastName.Length > NameMaxLength)
                validationResult.AddValidationItem(ValidationItems.Student.LastNameMaxLength);

            if (string.IsNullOrWhiteSpace(Email))
                validationResult.AddValidationItem(ValidationItems.Student.EmailNull);
            else if (!Email.Contains("@"))
                validationResult.AddValidationItem(ValidationItems.Student.EmailWrongFormat);

            if (!DateOfBirth.HasValue)
            {
                validationResult.AddValidationItem(ValidationItems.Student.DateOfBirthNull);
            }
            else
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);

                if (DateOfBirth.Value > today)
                    validationResult.AddValidationItem(ValidationItems.Student.Future);
            }

            return validationResult;
        }

        public static ValidationResult ValidatePassword(string password)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrWhiteSpace(password))
                return validationResult;

            if (password.Length < 8)
                validationResult.AddValidationItem(ValidationItems.Student.PasswordMinimum);
            else if (!password.Any(char.IsDigit) || !password.Any(char.IsUpper))
                validationResult.AddValidationItem(ValidationItems.Student.PasswordFormat);

            return validationResult;
        }
    }
}
