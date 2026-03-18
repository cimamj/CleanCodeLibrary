

namespace CleanCodeLibrary.Application.Common.CacheKeys
{
    public static class Keys
    {
        public const string AllBooks = "all_books";
        public const string AllStudents = "all_students";
        public const string AllGenres = "all_genres";
        public const string TopBooks10 = "top_books10";
        public const string TotalCountKey = "total_count";
        public static string Book(int id) => $"book_{id}"; //Keys.Book(req.id) saljes kao key
    }
}
