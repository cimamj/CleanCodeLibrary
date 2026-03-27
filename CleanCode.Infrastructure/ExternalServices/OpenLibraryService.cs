
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Domain.DTOs.Books;
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
            //var response = await _httpClient.GetAsync($"https://openlibrary.org/isbn/{isbn}.json");

            //if (!response.IsSuccessStatusCode) 
            //    return null;

            //var json = await response.Content.ReadAsStringAsync();

            //var data = JsonSerializer.Deserialize<OpenLibraryResponse>(json);

            //if (data == null) return null;

            //return new BookExternalDto
            //{
            //    Author = data.author,
            //    Title = data.title,
            //    Year = data.publish_date,
            //    Genre = data.genre
            //};
            
    }
}
