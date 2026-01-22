using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using Microsoft.VisualBasic;
using CleanCodeLibrary.Domain.Persistance.Common;

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

        public async Task<Result<int?>> BorrowBook(IUnitOfWork unitOfWork)
        {

            var validationResult = await BorrowValidation(unitOfWork);

            if (validationResult.HasError)
            {
                return new Result<int?>(null, validationResult);
            }

            await unitOfWork.BorrowRepository.InsertAsync(this);  
            return new Result<int?>(this.Id, validationResult);
        }

        private async Task<ValidationResult> BorrowValidation(IUnitOfWork unitOfWork)
        {
            var vr = new ValidationResult();

            // Provjere preko repozitorija iz UnitOfWork
            var bookExists = await unitOfWork.BookRepository.GetById(BookId) != null;
            if (!bookExists)
                vr.AddValidationItem(ValidationItems.Borrow.NoBookFound);

            var studentExists = await unitOfWork.StudentRepository.GetById(StudentId) != null;
            if (!studentExists)
                vr.AddValidationItem(ValidationItems.Borrow.NoStudentFound);

            var isBorrowed = await unitOfWork.BorrowRepository.IsBookCurrentlyBorrowed(BookId);
            if (isBorrowed)
                vr.AddValidationItem(ValidationItems.Borrow.BookBorrowed);

            return vr;
        }
    }
}
