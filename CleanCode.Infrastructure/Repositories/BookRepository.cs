using CleanCodeLibrary.Application.Students.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCode.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CleanCode.Infrastructure.Repositories
{
    public class BookRepository : Repository<Book, int>, IBookRepository
    {
        private readonly ApplicationDbContext _dbContext; 
        private readonly IDapperManager _dapperManager; 

        public BookRepository(DbContext dbContext, IDapperManager dapperManager)
            : base(dbContext)
        {
            _dapperManager = dapperManager ?? throw new ArgumentNullException(nameof(dapperManager));

            _dbContext = dbContext as ApplicationDbContext
                ?? throw new ArgumentException("DbContext must be ApplicationDbContext");
        }
        public async Task<Book> GetById(int id)
        {
           return await _dbContext.Books.FindAsync(id);
        }

        public async Task<Book> GetByTitle(string title)
        {
           //return await _dbContext.Books.FindAsync(title); jel moze primati ovo
           return await _dbContext.Books.FirstOrDefaultAsync(b=>b.Title == title); 
        }
    }
}
