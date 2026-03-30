using CleanCodeLibrary.Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeLibrary.Domain.DTOs.Books
{
    public class BookExternalDto
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int? Year { get; set; }
        public GenresEnum Genre { get; set; }        
    }
}
