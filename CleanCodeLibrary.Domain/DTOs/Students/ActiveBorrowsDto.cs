
using CleanCodeLibrary.Domain.Entities.Books;

namespace CleanCodeLibrary.Domain.DTOs.Students
{
    public class ActiveBorrowsDto
    {
        public DateOnly BorrowDate { get; set; } 
        public DateOnly DueDate { get; set; } 
        public DateOnly? ReturnDate { get; set; }
        public int AmountBorrowed { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public string Title { get; init; }
        public string Author { get; init; }
        public int Year { get; init; }
        public GenresEnum Genre { get; init; }

    }
}
