

namespace CleanCodeLibrary.Domain.Common.Validation.ValidationItems
{
    public static partial class ValidationItems
    {
        public static class Book
        {

            public static string CodePrefix = nameof(Book);


            public static readonly ValidationItem NameNull = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Naziv ne smije biti prazan",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };


            public static readonly ValidationItem TitleMaxLength = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Ime ne smije biti duze od {Entities.Books.Book.TitleNameMaxLength} znakova",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation,
            };


            public static readonly ValidationItem AuthorMaxLength = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Ime ne smije biti duze od {Entities.Books.Book.AuthorNameMaxLength} znakova",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation,
            };

            public static readonly ValidationItem IsbnMaxLength = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Ime ne smije biti duze od {Entities.Books.Book.IsbnMaxLength} znakova",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation,
            };


            public static readonly ValidationItem No_Books = new ValidationItem
            {
                Code = $"NO_BOOKS",
                Message = $"Nema nijedne knjige u bazi",
                ValidationSeverity = ValidationSeverity.Warning, //ili error
                ValidationType = ValidationType.FormalValidation
            };
            public static readonly ValidationItem No_Book = new ValidationItem
            {
                Code = $"NO_WANTED_BOOK",
                Message = $"Nema trazene knjige u bazi",
                ValidationSeverity = ValidationSeverity.Warning, //ili error
                ValidationType = ValidationType.FormalValidation
            };
        }
    }
}
