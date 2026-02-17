
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Persistance.Books;

namespace CleanCodeLibrary.Domain.Entities.Books
{
    public class Book
    {
        public const int TitleNameMaxLength = 200;
        public const int AuthorNameMaxLength = 150;
        public const int IsbnMaxLength = 20;
        public int Id { get; set; }

        public string Title = string.Empty;
        public string Author = string.Empty;
        public string? Isbn = string.Empty;
        public int Year { get; set; }
        public GenresEnum Genre { get; set; } //ovo je enum ef core automatski mapira genresenum u int, enum i je int ispod haube
        //polje za kolicinu ovakvih
        //STO ako ovog polja uopce nema u bazu a vec sam spojio bazu sa ovim backendom, jel se da to azurirat ili moram tamo rucno takoder ALTER TABLE ...add column...?
        public int Amount { get; set; }

        public async Task<ResultDomain<int?>> Create(IBookRepository bookRepository)
        {
            var validationResult = await CreateOrUpdateValidation(bookRepository);
            if (validationResult.HasError)
            {
                //Necemo zvat insert uopce
                return new ResultDomain<int?>(this.Id, validationResult);
            }
            await bookRepository.InsertAsync(this);
            return new ResultDomain<int?>(this.Id, validationResult);
        }

        public async Task<ResultDomain<int?>> Update(IBookRepository bookRepository)
        {
            var validationResult = await CreateOrUpdateValidation(bookRepository); 
            if (validationResult.HasError)
            {
                return new ResultDomain<int?>(null, validationResult);

            }
            bookRepository.Update(this); 
            return new ResultDomain<int?>(this.Id, validationResult);
        }

        //public static async Task<Result<Book>> GetByIdDomainAsync(IBookRepository bookRepository, int id)
        //{
        //    var existingBook = await bookRepository.GetById(id);
        //    var validationResult = new ValidationResult();
        //    if (existingBook == null)
        //    {
        //        validationResult.AddValidationItem(ValidationItems.Book.No_Book); //kriva konvecija pascal case  ide NoBook
        //    }
        //    return new Result<Book?>(existingBook, validationResult);
        //}

        public static async Task<ResultDomain<GetAllResponse<Book>>> GetAllBooksAsync(IBookRepository bookRepository)
        {
            var allBooks = await bookRepository.Get();

            var validationResult = new ValidationResult(); 
            if (allBooks == null || allBooks.Values == null || !allBooks.Values.Any())             {
                validationResult.AddValidationItem(ValidationItems.Book.No_Books);
            }
            return new ResultDomain<GetAllResponse<Book>>(allBooks, validationResult);

        }

        public async Task<ResultDomain<int?>> Delete(IBookRepository bookRepository) //NULLABLE REFERENCE JESE STA TRIBA DIRAT U RESULT KLASI
        {
            var deleteResult = await bookRepository.DeleteAsync(Id);
            var validationResult = new ValidationResult();

            if (!deleteResult)
            {
                validationResult.AddValidationItem(ValidationItems.Book.DeleteWentWrong);
                return new ResultDomain<int?>(null, validationResult);
            }
 
            return new ResultDomain<int?>(Id, validationResult);
        }

        public static async Task<ResultDomain<Book>> GetByTitle(IBookRepository bookRepository, string title)
        {
            var validationResult = new ValidationResult();
            var existingBookTitle = await bookRepository.GetByTitle(title);//gotova metoda kako se zove
            if (existingBookTitle == null)
            {
                validationResult.AddValidationItem(ValidationItems.Book.NotFound);
                return new ResultDomain<Book>(null, validationResult);
            }
            return new ResultDomain<Book>(existingBookTitle, validationResult);
        }

        public async Task SaveChanges(IBookRepository bookRepository)
        {
            await bookRepository.SaveAsync();
        }
        public async Task<ValidationResult> CreateOrUpdateValidation(IBookRepository bookRepository)
        {
            var validationResult = new ValidationResult();

            if(string.IsNullOrWhiteSpace(Title))
            {
                validationResult.AddValidationItem(ValidationItems.Book.TitleNull);
            }
            else if (Title.Length > TitleNameMaxLength)
            {
                validationResult.AddValidationItem(ValidationItems.Book.TitleMaxLength);
            }

            if (string.IsNullOrWhiteSpace(Author))
            {
                validationResult.AddValidationItem(ValidationItems.Book.AuthorNull);
            }

            else if (Author.Length > AuthorNameMaxLength)
            {
                validationResult.AddValidationItem(ValidationItems.Book.AuthorMaxLength);
            }

            if (string.IsNullOrWhiteSpace(Isbn))
            {
                validationResult.AddValidationItem(ValidationItems.Book.IsbnNull);
            }

            else if (Isbn.Length > IsbnMaxLength)
            {
                validationResult.AddValidationItem(ValidationItems.Book.IsbnMaxLength);
            }
            
            else
            {
                    var isbnTaken = await bookRepository.IsbnExistsForOtherBook(Isbn, Id);
                    if (isbnTaken)
                    {
                        validationResult.AddValidationItem(ValidationItems.Book.IsbnAlreadyExists);
                    }
            }
            

            if (Amount <= 0)
            {
                validationResult.AddValidationItem(ValidationItems.Book.AmountNullNegative);
            }

            if (!Enum.IsDefined(typeof(GenresEnum), Genre))
            {
                validationResult.AddValidationItem(ValidationItems.Book.UnknownGenre);
            }

            //kako da dodam provejru postoji li vec book sa ovim isbn? jer je ono unique pa baca 500, trebam li uopce to dodavati?
            //E SAD JE POSLOVNA LOGIKA DETALJNIJA SVE STA TI TREBA IMPLEMENTRIAS U INTERFACE , konkretno provjera postoji li isbn vec
           

            return validationResult;
        }
    }
}
