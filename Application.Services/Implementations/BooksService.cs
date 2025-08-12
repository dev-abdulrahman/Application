using Application.Entities.Models;
using Application.Repositories.Interfaces;
using Application.Services.Attributes;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Implementations
{
    [RegisterServiceAttribute(ServiceLifetime.Scoped)]
    public class BooksService : IBooksService
    {
        private readonly IGenericRepository<Book> _bookRepository;

        public BooksService(IUnitOfWork unitOfWork)
        {
            _bookRepository = unitOfWork.GetRepository<Book>();
        }

        public async Task AddBook(Book book)
        {
            await _bookRepository.Add(book);
        }

        public async Task DeleteBook(int bookId)
        {
            await _bookRepository.Delete(bookId);
        }

        public async Task UpdateBook(Book book)
        {
            _bookRepository.Update(book);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAll(book => book.BorrowRecords!);
        }

        public async Task<Book> GetBookById(int bookId)
        {
            return await _bookRepository.GetFirstOrDefault(book => book.BookId == bookId, book => book.BorrowRecords);
        }
    }
}
