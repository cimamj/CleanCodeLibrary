using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Domain.DTOs.Books;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanCode.Infrastructure.ExternalServices
{
    public class OpenLibraryService : IBookExternalService
    {
        private readonly HttpClient _httpClient;

        public OpenLibraryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BookExternalDto?> GetBookByIsbnAsync(string isbn)
        {
            var cleanIsbn = isbn.Replace("-", "").Trim();

            var response = await _httpClient.GetAsync(
                $"https://openlibrary.org/api/books?bibkeys=ISBN:{cleanIsbn}&format=json&jscmd=data");

            if (!response.IsSuccessStatusCode)
                return null;

            var dataDict = await response.Content
                .ReadFromJsonAsync<Dictionary<string, JsonElement>>();

            if (dataDict == null || dataDict.Count == 0)
                return null;

            var book = dataDict.First().Value;

            var title = book.TryGetProperty("title", out var titleEl)
                ? titleEl.GetString() ?? ""
                : "";

            var author = "";

            if (book.TryGetProperty("authors", out var authorsEl))
            {
                var first = authorsEl.EnumerateArray().FirstOrDefault();

                if (first.ValueKind != JsonValueKind.Undefined)
                    author = first.TryGetProperty("name", out var nameEl)
                        ? nameEl.GetString() ?? ""
                        : "";
            }

            var year = 0;

            if (book.TryGetProperty("publish_date", out var yearEl))
            {
                var yearStr = yearEl.GetString() ?? "";

                var yearMatch = System.Text.RegularExpressions.Regex.Match(yearStr, @"\d{4}");

                if (yearMatch.Success)
                    int.TryParse(yearMatch.Value, out year);
            }

            List<string> subjects = new List<string>();

            if (book.TryGetProperty("subjects", out var subjectsEl))
            {
                subjects = subjectsEl.EnumerateArray()
                    .Select(s => s.TryGetProperty("name", out var nameEl)
                        ? nameEl.GetString() ?? ""
                        : "")
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();
            }

            var mappedGenre = subjects.ToGenresEnum();

            return new BookExternalDto
            {
                Title = title,
                Author = author,
                Year = year,
                Genre = mappedGenre
            };
        }
    }
}
