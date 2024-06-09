using BookBridge.Application.Models.Request;

namespace BookBridge.Application.Interfaces
{
    public interface IBorrowService
    {
        Task<BorrowRecordModel> BorrowBookAsync(long bookId, string userId);
        Task<BorrowRecordModel> ReturnBookAsync(long bookId, string userId);
        Task<NotificationModel> CreateNotificationAsync(string userId, string message);
        Task<NotificationModel> MarkNotificationAsSentAsync(long notificationId);
        Task<IEnumerable<NotificationModel>> GetUserNotificationsAsync(string userId);
        Task DeleteNotificationAsync(long notificationId);
        Task<IEnumerable<NotificationModel>> CreateNotificationsAsync(IEnumerable<string> userIds, string message);
    }
}
