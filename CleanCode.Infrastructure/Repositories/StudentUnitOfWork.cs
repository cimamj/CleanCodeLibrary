using CleanCode.Infrastructure.Database;
using CleanCodeLibrary.Domain.Persistance.Students;


namespace CleanCode.Infrastructure.Repositories
{
    internal class StudentUnitOfWork : IStudentUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public IStudentRepository Repository { get; set; }

        public StudentUnitOfWork(ApplicationDbContext dbContext, IStudentRepository repository)
        {
            _dbContext = dbContext;
            Repository = repository;
        }
    }
}
