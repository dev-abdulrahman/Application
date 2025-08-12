using Application.Entities.Models;

namespace Application.Services.Interfaces
{
    public interface IBooksService
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(int bookId);
        Task AddBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(int bookId);
    }
}
