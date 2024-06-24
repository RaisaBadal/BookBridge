using BookBridge.Domain.Entities;

namespace BookBridge.Domain.Interfaces
{
    public interface IBorrowRecord
    {
        Task<BorrowRecord> BorrowBookAsync(long bookId, string userId);
        Task<BorrowRecord> ReturnBookAsync(long bookId, string userId);
        Task<IEnumerable<BorrowRecord>> ReturnAllActiveBorrowRecordAsync();
        Task<IEnumerable<BorrowRecord>> ReturnAllBorrowRecordAsync();
    }
}
