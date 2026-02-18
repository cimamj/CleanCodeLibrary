

using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Students;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.Persistance.Common;

namespace CleanCodeLibrary.Domain.Persistance.Students
{
    public interface IStudentRepository : IRepository<Student, int> //npr ovo IRepository u mojoj glavi ne //ovo nasljedivanje nuzno jer se ovime definira genericki tip kojim ce se baratat!! NE NEGO SE U REPOZITORIJU U INFRASTRUKTURI BARATA S TIPOM
        //triba nasljediti, jer kad definiras  u application ili domain layeru ovaj tip tj refernecu na repozitoij , tipa .insertasync nemos koristiti jer ovo samo po sebi nema nista, takoder s ovim baratamo u ta 2 slojaa korisimo njegove metode od parenta
    {
           Task<Student> GetById(int id);
           Task<GetAllResponse<StudentDto>> GetAllStudentDtos();

        Task<StudentDto> GetDtoById(int id);
        //// Dodaj specifične metode, npr:
        //Task<Student> GetByLastName(string lastName);
    }
}
