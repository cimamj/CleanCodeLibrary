using CleanCodeLibrary.Application.Students.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCode.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CleanCode.Infrastructure.Repositories
{
    public class BookRepository : Repository<Book, int>, IBookRepository
    {
        private readonly ApplicationDbContext _dbContext; 

        public BookRepository(DbContext dbContext)
            : base(dbContext)
        {

            _dbContext = dbContext as ApplicationDbContext
                ?? throw new ArgumentException("DbContext must be ApplicationDbContext");
        }
        public async Task<Book> GetById(int id)
        {
            await _dbContext.Books.AnyAsync(x => x.Id == id);
           return await _dbContext.Books.FindAsync(id);
        }

        public async Task<Book> GetByTitle(string title)
        {
           //return await _dbContext.Books.FindAsync(title); jel moze primati ovo
           return await _dbContext.Books.FirstOrDefaultAsync(b=>b.Title == title); 
        }

        public async Task<BookTitleDto?> GetNameById(int id)
        {
            //var book = await _dbContext.Books.SingleOrDefaultAsync(x => x.Id == id); //korsitim kad dohvacam unikatan properti iz baze, ako nade 2 entiteta s istim titlom PUKNIT CE BACIT CE 500, NECE UC U NULL, KORISITM ZA ID
            //var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Amount == amount); //retci tj entiteti koji nemaju unikatne propertije npr amount nije unikatan
            //var book = await _dbContext.Books.FindAsync(id); //trazi u memoriji jel postoji entitet,ako se vec dohvatia, ako je vadi ga iz memorije inace udara sql upit , na update ga iman


            //var book = await _dbContext.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Amount == amount); //retci tj entiteti koji nemaju unikatne propertije npr amount nije unikatan
            ////sto god dohvatin, on to stavlja u tracker i time prati koji su promjenjeni, i ako neman asnotracking kazem mu ne ulazi u tracker , tipa u get mi ne triba 

            var book = await _dbContext.Books
                .Where(x => x.Id == id)
                .Select(x => new BookTitleDto
                {
                    Title = x.Title
                })
                .SingleOrDefaultAsync();

            var bookEntity = await _dbContext.Books
                .SingleOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return null;
            }
            var bookTitle = new BookTitleDto
            {
                Title = book.Title
            };

            return bookTitle;
        }
    }
}
