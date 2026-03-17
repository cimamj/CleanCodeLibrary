
namespace CleanCodeLibrary.Domain.DTOs.Students
{
    public class BorrowStatisticsDto
    {
        public int TotalBorrows { get; set; }  //ukupno redaka borrow za tog studenta
        public int TotalBooksBorrowed { get; set; } //kolicina amount
        public string MostBorrowedBookTitle { get; set; } //koji redak knjige se najvise pojajvljuje u retcima borrows 
        public int MostBorrowedCount { get; set; } //koliko puta taj title
        public double AverageBorrowDays { get; set; }
        public int LateReturns { get; set; }
    }
}
