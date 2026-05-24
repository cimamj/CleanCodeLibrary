namespace CleanCodeLibrary.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        int? GetStudentId();
        string GetRole();
        bool IsAuthenticated();
        bool IsAdmin();
    }
}
