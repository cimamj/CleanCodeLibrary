using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.Persistance.Common;

namespace CleanCodeLibrary.Domain.Persistance.Students
{
    public interface IStudentRepository : IRepository<Student, int>
    {
        Task<Student> GetById(int id);

        Task<GetAllResponse<StudentDto>> GetAllStudentDtos();

        Task<StudentDto> GetDtoById(int id);

        Task<GetAllResponse<ActiveBorrowsDto>> GetActiveBorrowsDtos(int id);

        Task<bool> IsEmailTaken(string email, int currentId);

        Task<Student> GetByEmail(string email);
    }
}
