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

        public StudentRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

            _dbContext = dbContext;
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
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        DateOfBirth = s.DateOfBirth
                    })
                    .SingleOrDefaultAsync();
            return student;
        }

        public async Task<GetAllResponse<ActiveBorrowsDto>> GetActiveBorrowsDtos(int id)
        {
            var activeBorrows = await _dbContext.Borrows
                .Where(x => x.StudentId == id && x.ReturnDate == null)
                .Select(x => new ActiveBorrowsDto
                {
                    BorrowDate = x.BorrowDate,
                    DueDate = x.DueDate,
                    ReturnDate = x.ReturnDate,
                    AmountBorrowed = x.AmountBorrowed,
                    FirstName = x.Student.FirstName,
                    LastName = x.Student.LastName,
                    DateOfBirth = x.Student.DateOfBirth,
                    Title = x.Book.Title,
                    Author = x.Book.Author,
                    Year = x.Book.Year,
                    Genre = x.Book.Genre
                })
                .ToListAsync();
            //na get i sa ovim selectom , ne triba include
            //bez select nebi moga pristupit first
            //da ga zelim edit update triban ga include

            return new GetAllResponse<ActiveBorrowsDto> { Values = activeBorrows };
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