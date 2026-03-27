using CleanCodeLibrary.Domain.DTOs.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeLibrary.Application.Common.Interfaces
{
    public interface IBookExternalService
    {
        Task<BookExternalDto?> GetBookByIsbnAsync(string isbn);
    }
}
