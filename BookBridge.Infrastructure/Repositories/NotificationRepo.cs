using BookBridge.Domain.Data;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Infrastructure.Repositories
{
    public class NotificationRepo:AbstractClass<Notification>,INotificationRepo
    {
        public NotificationRepo(BookBridgeDb context) : base(context)
        {
        }

        #region CreateNotificationAsync
        public async Task<Notification> CreateNotificationAsync(string userId, string message)
        {
            var user = await Context.Users.FindAsync(userId);
            if (user is null)
            {
                throw new KeyNotFoundException($"User with id: {userId} is not found.");
            }

            var newMessage = new Notification()
            {
                CreatedDate = DateTime.Now,
                Message = message,
                UserId = userId
            };
            await DbSet.AddAsync(newMessage);
            await Context.SaveChangesAsync();

            return newMessage;
        }
        #endregion

        #region MarkNotificationAsSentAsync

        public async Task<Notification> MarkNotificationAsSentAsync(long notificationId)
        {
            var notification = await DbSet.FindAsync(notificationId) 
                               ?? throw new KeyNotFoundException($"Notification not found by id: {notificationId}");
            notification.SentDate= DateTime.Now;
            notification.IsSent = true;
            await Context.SaveChangesAsync();
            return notification;
        }


        #endregion

        #region GetUserNotificationsAsync

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            var user = await Context.Users.FindAsync(userId)
                       ?? throw new KeyNotFoundException($"User not found by id: {userId}");
            var notification = await DbSet.Where(i=>i.UserId==userId).ToListAsync();
            if (notification is null) throw new Exception("No Notification found");
            return notification;
        }
        #endregion

        #region DeleteNotificationAsync

        public async Task DeleteNotificationAsync(long notificationId)
        {
            var notification = await DbSet.FindAsync(notificationId) 
                               ?? throw new KeyNotFoundException($"No message found by id: {notificationId}");
            DbSet.Remove(notification);
            await Context.SaveChangesAsync();

        }
        #endregion
    }
}
