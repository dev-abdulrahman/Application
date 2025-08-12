using Application.Entities.Models;

namespace Application.Services.Interfaces
{
    public interface IBorrowService
    {
        Task<IEnumerable<BorrowRecord>> GetAllBorrowedBooks();
        Task<BorrowRecord> GetBorrowedBookById(int borrowedId);
        Task AddBorrowedBook(BorrowRecord book);
        Task UpdateBorrowedBook(BorrowRecord book);
    }
}
