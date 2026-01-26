using CleanCode.Infrastructure.Database;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.Persistance.Students;
using Microsoft.EntityFrameworkCore;
using CleanCodeLibrary.Domain.Common.Model; // za GetByIdRequest ako želiš koristiti

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
            return await _dbContext.Students.FindAsync(id);

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
    }
}