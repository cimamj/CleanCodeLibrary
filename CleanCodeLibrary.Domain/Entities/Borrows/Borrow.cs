using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Entities.Students;

namespace CleanCodeLibrary.Domain.Entities.Borrows
{
    public class Borrow
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

        public DateOnly BorrowDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        public DateOnly DueDate { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public int AmountBorrowed { get; set; }

        public ValidationResult ValidateReturn()
        {
            var validationResult = new ValidationResult();

            if (ReturnDate != null)
                validationResult.AddValidationItem(ValidationItems.Borrow.AlreadyReturned);

            if (AmountBorrowed <= 0)
            {
                validationResult.AddValidationItem(new ValidationItem
                {
                    Message = "Posuđena količina mora biti veća od 0",
                    ValidationSeverity = ValidationSeverity.Error
                });
            }

            return validationResult;
        }
    }
}
