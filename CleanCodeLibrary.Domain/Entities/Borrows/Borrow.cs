using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.Persistance.Borrows;

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
        public DateOnly DueDate { get; set; } //ne provjeravam je li kasni
        public DateOnly? ReturnDate { get; set; }
        public int AmountBorrowed { get; set; } //Makli smo kao argument odvojeni int amount

        public async Task<ResultDomain<int?>> BorrowBook(IBorrowUnitOfWork unitOfWork)  
        {
          
            var validationResult = await BorrowValidation(unitOfWork);

            if (validationResult.HasError)
            {
                return new ResultDomain<int?>(null, validationResult);
            }
           
            await unitOfWork.BorrowRepository.InsertBorrow(this, AmountBorrowed);
            await unitOfWork.BookRepository.DecrementAmount(BookId, AmountBorrowed);
            await unitOfWork.BookRepository.IncrementBorrowCount(BookId, AmountBorrowed);
            return new ResultDomain<int?>(Id, validationResult);
        }

        //metoda za vratit knjigu, u request dto ide borrow id jer mozes istu knjigu vise puta posudit
        public async Task<ResultDomain<int?>> Return(IBorrowUnitOfWork unitOfWork)
        {
            var validationResult = new ValidationResult();

            var repository = unitOfWork.BorrowRepository;

            var existingBorrow = await repository.GetById(Id);
            if (existingBorrow == null)
                validationResult.AddValidationItem(ValidationItems.Borrow.BorrowNotFound);

            if (validationResult.HasError)
                return new ResultDomain<int?>(null, validationResult);


            if (existingBorrow.ReturnDate != null)
            {
                validationResult.AddValidationItem(ValidationItems.Borrow.AlreadyReturned);
            }

            if (existingBorrow.AmountBorrowed <= 0)
            {
                validationResult.AddValidationItem(new ValidationItem
                {
                    Message = "Posuđena količina mora biti veća od 0",
                    ValidationSeverity = ValidationSeverity.Error
                });
            }

            //ReturnDate = DateOnly.FromDateTime(DateTime.UtcNow);
            //AmountBorrowed = 0; glup si ko kurac ostavi kolko je bilo
            if (validationResult.HasError)
                return new ResultDomain<int?>(null, validationResult);

            await unitOfWork.BorrowRepository.UpdateBorrow(this);

            return new ResultDomain<int?>(Id, validationResult);
        }


        public async Task<ValidationResult> BorrowValidation(IBorrowUnitOfWork unitOfWork)
        {
            var vr = new ValidationResult();

            //provjere preko repozitorija iz UnitOfWork
            var bookExists = await unitOfWork.BookRepository.GetById(BookId);
            
            if (bookExists == null)
                vr.AddValidationItem(ValidationItems.Book.NotFound);
            else if(bookExists.Amount < AmountBorrowed) //doda ovu provjeru za TRANSAKCIJU, ako je na lageru vise ili jednako,mos posudit, inace ne
                vr.AddValidationItem(ValidationItems.Borrow.NotEnoughBooks(bookExists.Amount, AmountBorrowed)); //Brutalnooooooooo

            var studentExists = await unitOfWork.StudentRepository.GetById(StudentId) != null;
            if (!studentExists)
                vr.AddValidationItem(ValidationItems.Borrow.NoStudentFound);

            var isBorrowed = await unitOfWork.BorrowRepository.IsBookCurrentlyBorrowed(BookId);
            if (isBorrowed)
                vr.AddValidationItem(ValidationItems.Borrow.BookBorrowed);

            return vr;
        }
        //public async Task<ValidationResult> ReturnValidation(IUnitOfWork unitOfWork)
        //{
        //    var vr = new ValidationResult();

        //    var borrow = await unitOfWork.BorrowRepository.GetById(Id); //id od this tj request
        //    if (borrow == null || borrow.ReturnDate != null) //ako postoji returndate od this a this ima id
        //        vr.AddValidationItem(ValidationItems.Borrow.AlreadyReturned);

        //    return vr;
        //}


    }
}
