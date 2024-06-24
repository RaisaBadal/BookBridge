using BookBridge.Application.Models;
using BookBridge.Application.Models.Request;

namespace BookBridge.Application.Interfaces
{
    public interface IBorrowService
    {
        Task<BorrowRecordModel> BorrowBookAsync(long bookId, string userId);
        Task<BorrowRecordModel> ReturnBookAsync(long bookId, string userId);
        Task<IEnumerable<BorrowRecordModel>> GetUserBorrowRecordsAsync(string userId);
        Task<bool> UpdateDueDateAsync(UpdateBorrowRecordModel updateBorrowRecordModel);
        Task SendRemindersAsync();
    }
}
