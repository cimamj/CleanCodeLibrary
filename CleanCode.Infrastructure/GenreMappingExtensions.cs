

using CleanCodeLibrary.Domain.Entities.Books;

namespace CleanCode.Infrastructure
{
    public static class GenreMappingExtensions
    {
        public static GenresEnum ToGenresEnum(this List<string> subjects)
        {
            if (subjects == null || subjects.Count == 0)
                return GenresEnum.Fiction;

            var lowerSubjects = subjects.Select(s => s.ToLowerInvariant()).ToList();

            foreach (var subject in lowerSubjects)
            {
                if (subject.Contains("fantasy") || subject.Contains("fantast"))
                    return GenresEnum.Fantasy;

                if (subject.Contains("sci-fi") ||
                    subject.Contains("science fiction") ||
                    subject.Contains("scifi") ||
                    subject.Contains("science-fiction"))
                    return GenresEnum.SciFi;

                if (subject.Contains("mystery") ||
                    subject.Contains("detective") ||
                    subject.Contains("crime") ||
                    subject.Contains("thriller"))
                    return GenresEnum.Mystery;

                if (subject.Contains("horror"))
                    return GenresEnum.Horror;

                if (subject.Contains("romance") || subject.Contains("ljubav"))
                    return GenresEnum.Romance;

                if (subject.Contains("biography") ||
                    subject.Contains("autobiography") ||
                    subject.Contains("memoar"))
                    return GenresEnum.Biography;

                if (subject.Contains("history") || subject.Contains("povijest"))
                    return GenresEnum.History;

                if (subject.Contains("non-fiction") ||
                    subject.Contains("nonfiction") ||
                    subject.Contains("stručna") ||
                    subject.Contains("non fiction"))
                    return GenresEnum.NonFiction;
            }

            return GenresEnum.Fiction;
        }
    }

}