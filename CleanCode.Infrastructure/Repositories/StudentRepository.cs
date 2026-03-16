using CleanCode.Infrastructure.Database;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.Persistance.Students;
using Microsoft.EntityFrameworkCore;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Students;

namespace CleanCode.Infrastructure.Repositories
{
    public class StudentRepository : Repository<Student, int>, IStudentRepository
    {
        private readonly ApplicationDbContext _dbContext; //zasto bas application, a ne obicni kao u parentu
        private readonly IDapperManager _dapperManager;

        public StudentRepository(ApplicationDbContext dbContext, IDapperManager dapperManager)
            : base(dbContext)
        {

            _dbContext = dbContext;
            _dapperManager = dapperManager;
        }

        public async Task<Student?> GetById(int id) //mozda ovi argument cisci GetByIdRequest<int> request
        {
            //EF nacin
            //return await _dbSet.FindAsync(id); //_dbSet je privatno polje
            return await _dbContext.Students.FindAsync(id); //vraca pravi entitet ne dto!!!!

            //ili .FirstOrDefaultAsync(s => s.Id == id);

            // === Dapper nacin SQL
            /*
            return await _dapperManager.QuerySingleAsync<Student>(
                @"SELECT id AS Id, 
                         firstname AS FirstName, 
                         lastname AS LastName, 
                         dateofbirth AS DateOfBirth 
                  FROM Students 
                  WHERE id = @Id",
                new { Id = id }
            );
            */

            /*
             var sql = 
             """
                SELECT 
                    id as Id,
                    firstname AS FirstName, 
                    lastname AS LastName, 
                    dateofbirth AS DateOfBirth 
                FROM public.Students
                WHERE id = @Id                              //AND name = Name, Name dodas u parametar doli
            """;

            var parameters = new 
            {
            Id = id
            };

           
            return await _dapperManager.QuerySingleAsync<Student>(sql, parameters)
             
             
             */

        }



        public async Task<GetAllResponse<StudentDto>> GetAllStudentDtos()
        {
            var studentDtos = await _dbContext.Students
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    DateOfBirth = s.DateOfBirth,
                })
                .ToListAsync();

            return new GetAllResponse<StudentDto> { Values = studentDtos };
        }



        public async Task<StudentDto> GetDtoById(int id)
        {
            //var student = await _dbContext.Students
            //    .FindAsync(id);
            //return new StudentDto { FirstName = student.FirstName, LastName = student.LastName, DateOfBirth = student.DateOfBirth, };
            //nemozes ovako, puca ako ne nade s tim id, vraca null i onda u novoj liniji mapiras novi objekt sa null objektom, triba bi projverit je li null
            var student = await _dbContext.Students
                    .Where(s => s.Id == id)
                    .Select(s => new StudentDto
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        DateOfBirth = s.DateOfBirth
                    })
                    .SingleOrDefaultAsync();
            return student;
        }

        public async Task<GetAllResponse<ActiveBorrowsDto>> GetActiveBorrowsDtos(int id)
        {
            //var activeBorrows = await _dbContext.Borrows
            //    .Where(x => x.StudentId == id && x.ReturnDate == null)
            //    .Select(x => new ActiveBorrowsDto
            //    {
            //        Id = x.Id,
            //        BorrowDate = x.BorrowDate,
            //        DueDate = x.DueDate,
            //        ReturnDate = x.ReturnDate,
            //        AmountBorrowed = x.AmountBorrowed,
            //        FirstName = x.Student.FirstName,
            //        LastName = x.Student.LastName,
            //        DateOfBirth = x.Student.DateOfBirth,
            //        Title = x.Book.Title,
            //        Author = x.Book.Author,
            //        Year = x.Book.Year,
            //        Genre = x.Book.Genre
            //    })
            //    .ToListAsync();
            //na get i sa ovim selectom , ne triba include
            //bez select nebi moga pristupit first
            //da ga zelim edit update triban ga include

            const string sql = @"
            SELECT 
             b.Id,
             b.BorrowDate,
             b.DueDate,
             b.AmountBorrowed,
             bk.Title,
             bk.Author
            FROM Borrows b
            JOIN Books bk ON b.BookId = bk.Id
            WHERE b.StudentId = @Id 
              AND b.ReturnDate IS NULL
            ORDER BY b.BorrowDate DESC";
            var parameters = new
            {
                Id = id
            }; //ovo ide pod @


            var borrows = (await _dapperManager.QueryAsync<ActiveBorrowsDto>(sql, parameters)).ToList();
            //var result = borrows.Select(b => new ActiveBorrowsDto
            //{
            //    Id = b.Id,
            //    BorrowDate = DateOnly.FromDateTime(b.BorrowDate), //zali se jer je vec dateonly ovaj b.borrowsdate
            //    DueDate = DateOnly.FromDateTime(b.DueDate),
            //    AmountBorrowed = b.AmountBorrowed,
            //    Title = b.Title,
            //    Author = b.Author
            //}).ToList();


            return new GetAllResponse<ActiveBorrowsDto> { Values = borrows };
        }

        public async Task<bool> IsEmailUnique(string email, int currentId)
        {
            return await _dbContext.Students
                .AnyAsync(x => x.Email == email && x.Id != currentId);
            //AnyAsync
        }

        public async Task<Student> GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await _dbContext.Students
                  .SingleOrDefaultAsync(x => x.Email == email);
        }

    }
}