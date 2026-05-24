using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Entities.Borrows;

namespace CleanCodeLibrary.Domain.Entities.Books
{
    public class Book
    {
        public const int TitleNameMaxLength = 200;
        public const int AuthorNameMaxLength = 150;
        public const int IsbnMaxLength = 20;

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string? Isbn { get; set; }

        public int Year { get; set; }

        public GenresEnum Genre { get; set; }

        public int Amount { get; set; }

        public int BorrowCount { get; set; } = 0;

        public ICollection<Borrow> Borrows { get; set; }

        public ValidationResult Validate()
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrWhiteSpace(Title))
                validationResult.AddValidationItem(ValidationItems.Book.TitleNull);
            else if (Title.Length > TitleNameMaxLength)
                validationResult.AddValidationItem(ValidationItems.Book.TitleMaxLength);

            if (string.IsNullOrWhiteSpace(Author))
                validationResult.AddValidationItem(ValidationItems.Book.AuthorNull);
            else if (Author.Length > AuthorNameMaxLength)
                validationResult.AddValidationItem(ValidationItems.Book.AuthorMaxLength);

            if (string.IsNullOrWhiteSpace(Isbn))
                validationResult.AddValidationItem(ValidationItems.Book.IsbnNull);
            else if (Isbn.Length > IsbnMaxLength)
                validationResult.AddValidationItem(ValidationItems.Book.IsbnMaxLength);

            if (Amount <= 0)
                validationResult.AddValidationItem(ValidationItems.Book.AmountNullNegative);

            if (!Enum.IsDefined(typeof(GenresEnum), Genre))
                validationResult.AddValidationItem(ValidationItems.Book.UnknownGenre);

            if (Year <= 0)
                validationResult.AddValidationItem(ValidationItems.Book.NegativeYear);

            return validationResult;
        }
    }
}
