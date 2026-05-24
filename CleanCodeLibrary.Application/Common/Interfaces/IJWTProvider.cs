namespace CleanCodeLibrary.Application.Common.Interfaces
{
    public interface IJWTProvider
    {
        string GetSecretKey();
        string GetIssuer();
        string GetAudience();
        string GetExpiryHours();
    }
}
