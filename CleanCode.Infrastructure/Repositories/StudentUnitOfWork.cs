using CleanCode.Infrastructure.Database;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Students;


namespace CleanCode.Infrastructure.Repositories
{
    internal class StudentUnitOfWork : IStudentUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IStudentRepository StudentRepository {  get; set; }

        //public IBookRepository bookRepository { get; set; }
        //public IBorrowRepository borrowRepository { get; set; }

        public StudentUnitOfWork(ApplicationDbContext dbContext, IStudentRepository repository)
        {
            _dbContext = dbContext;
            StudentRepository = repository;
            //bookRepository = _bookRepository;
            //borrowRepository = _borrowRepository;   
        }
    }
}
