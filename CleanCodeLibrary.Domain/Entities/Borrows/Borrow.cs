using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using Microsoft.VisualBasic;
using CleanCodeLibrary.Domain.Persistance.Common;
using static CleanCodeLibrary.Domain.Common.Validation.ValidationItems.ValidationItems;

namespace CleanCodeLibrary.Domain.Entities.Borrows
{
    public class Borrow
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public async Task<Result<int?>> BorrowBook(IUnitOfWork unitOfWork, int amount) 
        {
            //dakle sad kad imam amount , ode moram ocito transkaciju radit, jer createm redak u Borrow i updateam redak u Books pod amount 
            //takoder dodajem dodatne provjere, pod amount > 0?

            var validationResult = await BorrowValidation(unitOfWork, amount);

            if (validationResult.HasError)
            {
                return new Result<int?>(null, validationResult);
            }
           
            await unitOfWork.BorrowRepository.InsertAsync(this);  
            return new Result<int?>(this.Id, validationResult);
        }

        //metoda za vratit knjigu, u request dto ide borrow id jer mozes istu knjigu vise puta posudit
        public static async Task<Result<int?>> Return(IUnitOfWork unitOfWork, int borrowId)
        {
            var validationResult = new ValidationResult();

            var borrow = await unitOfWork.BorrowRepository.GetById(borrowId);
            if (borrow == null)
            {
                validationResult.AddValidationItem(ValidationItems.Borrow.BorrowNotFound);
                return new Result<int?>(null, validationResult);
            }
            if (borrow.ReturnDate != null)
            {
                validationResult.AddValidationItem(ValidationItems.Borrow.AlreadyReturned);
                return new Result<int?>(null, validationResult);
            }

            borrow.ReturnDate = DateTime.UtcNow;
            unitOfWork.BorrowRepository.Update(borrow); //lakse ovako nego slat u fju za validaciju 

            return new Result<int?>(borrow.Id, validationResult);
        }


        public async Task<ValidationResult> BorrowValidation(IUnitOfWork unitOfWork, int amount)
        {
            var vr = new ValidationResult();

            //provjere preko repozitorija iz UnitOfWork
            var bookExists = await unitOfWork.BookRepository.GetById(BookId);
            if (bookExists == null)
                vr.AddValidationItem(ValidationItems.Borrow.NoBookFound);



            if(bookExists.Amount < amount) //doda ovu provjeru za TRANSAKCIJU, ako je na lageru vise ili jednako,mos posudit, inace ne
                vr.AddValidationItem(ValidationItems.Borrow.NotEnoughBooks(bookExists.Amount, amount)); //Brutalnooooooooo



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
