using CleanCode.Infrastructure.Database;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.Persistance.Students;
using Microsoft.EntityFrameworkCore;

namespace CleanCode.Infrastructure.Repositories
{
    public class StudentRepository : Repository<Student, int>, IStudentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDapperManager _dapperManager;

        public StudentRepository(ApplicationDbContext dbContext, IDapperManager dapperManager)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _dapperManager = dapperManager;
        }

        public async Task<Student?> GetById(int id)
        {
            return await _dbContext.Students.FindAsync(id);
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
            return await _dbContext.Students
                .Where(s => s.Id == id)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    DateOfBirth = s.DateOfBirth
                })
                .SingleOrDefaultAsync();
        }

        public async Task<GetAllResponse<ActiveBorrowsDto>> GetActiveBorrowsDtos(int id)
        {
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

            var borrows = (await _dapperManager.QueryAsync<ActiveBorrowsDto>(sql, new { Id = id })).ToList();

            return new GetAllResponse<ActiveBorrowsDto> { Values = borrows };
        }

        public async Task<bool> IsEmailTaken(string email, int currentId)
        {
            return await _dbContext.Students
                .AnyAsync(x => x.Email == email && x.Id != currentId);
        }

        public async Task<Student?> GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await _dbContext.Students
                .SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}
