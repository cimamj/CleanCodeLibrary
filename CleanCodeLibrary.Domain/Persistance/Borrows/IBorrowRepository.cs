using CleanCodeLibrary.Domain.Entities.Borrows;
using CleanCodeLibrary.Domain.Persistance.Common;

namespace CleanCodeLibrary.Domain.Persistance.Borrows
{
    public interface IBorrowRepository : IRepository<Borrow, int>
    {
        Task<Borrow> GetById(int id);
        Task<bool> IsBookCurrentlyBorrowed(int bookId);
        Task<int> InsertBorrow(Borrow borrow, int amount);
    }
}
