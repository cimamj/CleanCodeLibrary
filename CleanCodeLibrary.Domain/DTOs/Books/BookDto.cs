using CleanCodeLibrary.Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeLibrary.Domain.DTOs.Books
{
    public class BookDto
    {
        public string Title {  get; init; }
        public string Author { get; init; }
        public string Isbn { get; init; }
        public int Year { get; init; }
        public GenresEnum Genre { get; init; }

        //idiote poremeceni zaboravia si amonut polje dodat
        public int Amount { get; init; }
    }
}
