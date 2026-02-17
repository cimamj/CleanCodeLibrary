

namespace CleanCodeLibrary.Domain.Common.Validation.ValidationItems
{
    public static partial class ValidationItems
    {
        public static class Book
        {

            public static string CodePrefix = nameof(Book);


            public static readonly ValidationItem TitleNull = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Naslov ne smije biti prazan",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem AuthorNull = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Autor ne smije biti prazan",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem IsbnNull = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Isbn ne smije biti prazan",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem AmountNullNegative = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Kolicina knjiga mora biti veća od 0",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.FormalValidation
            };

            public static readonly ValidationItem UnknownGenre = new ValidationItem
            {
                Code = $"{CodePrefix}1",
                Message = $"Nepoznati zanr",
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
            public static readonly ValidationItem NotFound = new ValidationItem
            {
                Code = $"NO_WANTED_BOOK",
                Message = $"Nema trazene knjige u bazi",
                ValidationSeverity = ValidationSeverity.Warning, //ili error
                ValidationType = ValidationType.FormalValidation
            };

            
                 public static readonly ValidationItem DeleteWentWrong = new ValidationItem
                 {
                     Code = $"BOOK_NOT_DELETED",
                     Message = $"Knjiga se nije uspjela obrisati",
                     ValidationSeverity = ValidationSeverity.Error, //ili error
                     ValidationType = ValidationType.SystemError
                 };

        
             public static readonly ValidationItem IsbnAlreadyExists = new ValidationItem
             {
                 Code = $"{CodePrefix}.IsbnAlreadyExists",
                 Message = "Knjiga sa ovim ISBN brojem već postoji",
                 ValidationSeverity = ValidationSeverity.Error,
                 ValidationType = ValidationType.BussinessRule
             };
        }
    }
}
