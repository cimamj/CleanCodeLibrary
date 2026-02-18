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

        public StudentRepository(DbContext dbContext)
            : base(dbContext)
        {

            _dbContext = dbContext as ApplicationDbContext
                ?? throw new ArgumentException("DbContext must be ApplicationDbContext");
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

       
    }
}