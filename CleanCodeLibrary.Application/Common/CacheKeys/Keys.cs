

namespace CleanCodeLibrary.Application.Common.CacheKeys
{
    public static class Keys
    {
        public const string AllBooks = "all_books";
        public const string AllStudents = "all_students";
        public const string AllGenres = "all_genres";
        public static string Book(int id) => $"book_{id}";
    }
}
