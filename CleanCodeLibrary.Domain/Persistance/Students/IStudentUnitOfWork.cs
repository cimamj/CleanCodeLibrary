

namespace CleanCodeLibrary.Domain.Persistance.Students
{
    public interface IStudentUnitOfWork
    {
        IStudentRepository Repository { get; }
    }
}
 