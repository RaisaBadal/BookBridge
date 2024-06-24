using BookBridge.Domain.Entities;

namespace BookBridge.Domain.Interfaces
{
    public interface INotificationRepo
    {
        Task<Notification> CreateNotificationAsync(string message);

        Task<Notification> GetNotificationByIdAsync(long notificationId);

        Task<Notification> UpdateNotificationAsync(long id, Notification notification);

        Task<IEnumerable<UserNotification>> GetAllNotificationAsync();

        Task DeleteNotificationAsync(long notificationId);

        Task<bool> AtachNotificationToUserAsync(UserNotification userNotification);
    }
}
