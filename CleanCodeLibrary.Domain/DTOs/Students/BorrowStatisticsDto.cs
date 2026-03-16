
namespace CleanCodeLibrary.Domain.DTOs.Students
{
    public class BorrowStatisticsDto
    {
        public int TotalBorrows { get; set; }
        public int TotalBooksBorrowed { get; set; }
        public string MostBorrowedBookTitle { get; set; }
        public int MostBorrowedCount { get; set; }
        public double AverageBorrowDays { get; set; }
        public int LateReturns { get; set; }
    }
}
