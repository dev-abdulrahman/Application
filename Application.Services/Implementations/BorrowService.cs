using Application.Entities.Models;
using Application.Repositories.Interfaces;
using Application.Services.Attributes;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Implementations
{
    [RegisterServiceAttribute(ServiceLifetime.Scoped)]
    public class BorrowService : IBorrowService
    {
        private readonly IGenericRepository<BorrowRecord> _borrowRecordRepository;

        public BorrowService(IUnitOfWork unitOfWork)
        {
            _borrowRecordRepository = unitOfWork.GetRepository<BorrowRecord>();
        }

        public async Task AddBorrowedBook(BorrowRecord book)
        {
            await _borrowRecordRepository.Add(book);
        }

        public async Task<IEnumerable<BorrowRecord>> GetAllBorrowedBooks()
        {
            return await _borrowRecordRepository.GetAll(br => br.Book);
        }

        public async Task<BorrowRecord> GetBorrowedBookById(int borrowedId)
        {
            return await _borrowRecordRepository.GetFirstOrDefault(br => br.BorrowRecordId == borrowedId, br => br.Book);
        }

        public async Task UpdateBorrowedBook(BorrowRecord borrowRecord)
        {
             _borrowRecordRepository.Update(borrowRecord);
        }
    }
}
