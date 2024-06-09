using BookBridge.Domain.Entities;

namespace BookBridge.Domain.Interfaces
{
    public interface INotificationRepo
    {
        Task<Notification> CreateNotificationAsync(string userId, string message);
        Task<Notification> MarkNotificationAsSentAsync(long notificationId);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
        Task DeleteNotificationAsync(long notificationId);
    }
}
