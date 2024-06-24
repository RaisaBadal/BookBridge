using BookBridge.Application.Models.Request;
using BookBridge.Domain.Entities;

namespace BookBridge.Application.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationModel> CreateNotificationAsync(string message);

        Task<NotificationModel> GetNotificationByIdAsync(long notificationId);

        Task<NotificationModel> UpdateNotificationAsync(long id, NotificationModel notification);

        Task<IEnumerable<UserNotificationModel>> GetAllNotificationAsync();

        Task DeleteNotificationAsync(long notificationId);

        Task<bool> SendNotificationToUserAsync(long notificationId, string userId);

        Task<bool> SendNotificationToUsersAsync(long notificationId, List<string> usersIds);

        Task<bool> SendNotificationToAllUsersAsync(long notificationId);

        Task<bool> MarkNotificationAsSentAsync(long notificationId);

        Task<IEnumerable<NotificationModel>> GetUserNotificationsAsync(string userId);
    }
}
