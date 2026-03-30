
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

            if (!response.IsSuccessStatusCode)  //ako nije 200, tj ako nije stigao potreban dio contenta
                return null;

            
            var dataDict = await response.Content
                .ReadFromJsonAsync<Dictionary<string, JsonElement>>(); //direktno iz streama ne mora vuc u memoriju string, vuce odmah dictionary
            //sad je pravi objekt nad kojim se mogu udarat linq metode

            if (dataDict == null || dataDict.Count == 0)
                return null;

            var book = dataDict.First().Value; //.net nezna da je 1 par samo, stoga udri .first i daj desnu stranu samo 

            var title = book.TryGetProperty("title", out var titleEl)
                ? titleEl.GetString() ?? "" //moze vratiti null ako pise u json title:null
                : "";

            var author = "";
            if (book.TryGetProperty("authors", out var authorsEl))
            {
                var first = authorsEl.EnumerateArray().FirstOrDefault(); //sad je prebrojivo i mos udarat linq metode, dotad authorsEl je objekt tipa JsonEle
                if (first.ValueKind != JsonValueKind.Undefined) //ako nema autora ovo provjerajer ce valuekind bit undefined jer je jsonelement struct value type, ako ima to ce bit prvi par object key value 
                    author = first.TryGetProperty("name", out var nameEl)
                        ? nameEl.GetString() ?? ""
                        : "";
            }

            var year = 0;
            if (book.TryGetProperty("publish_date", out var yearEl))
            {
                var yearStr = yearEl.GetString() ?? ""; //jer je json string
                // pronadi 4 uzastopne znamenke da je smislena brojka , npr ako je "" vraca se 0
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
