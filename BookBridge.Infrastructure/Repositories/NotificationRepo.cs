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
        public async Task<Notification> CreateNotificationAsync(string message)
        {
            if (await DbSet.AnyAsync(i => i.Message == message))
            {
                throw new ArgumentException("Such message already exist in DB");
            }
            Notification notification = new Notification()
            {
                CreatedDate = DateTime.Now,
                Message = message,
            };
            await DbSet.AddAsync(notification);
            await Context.SaveChangesAsync();
            return notification;
        }
        #endregion

        #region DeleteNotificationAsync

        public async Task DeleteNotificationAsync(long notificationId)
        {
            var notification = await DbSet.FindAsync(notificationId)
                ?? throw new ArgumentException($"No notification found by id: {notificationId}");
            DbSet.Remove(notification);
            await Context.SaveChangesAsync();
        }
        #endregion

        #region GetAllNotificationAsync

        public async Task<IEnumerable<UserNotification>> GetAllNotificationAsync()
        {
            return await Context.UserNotifications.ToListAsync();
        }
        #endregion

        #region GetNotificationByIdAsync

        public async Task<Notification> GetNotificationByIdAsync(long notificationId)
        {
            var notification = await DbSet.FindAsync(notificationId)
                ?? throw new ArgumentException($"No notification find by id: {notificationId}");
            return notification;

        }
        #endregion

        #region UpdateNotificationAsync

        public async Task<Notification> UpdateNotificationAsync(long id,Notification notification)
        {
            var notif = await DbSet.FindAsync(id)
                 ?? throw new ArgumentException($"No notification found by id: {id}");
            notif.Message = notification.Message;
            notif.CreatedDate = DateTime.Now;
            await Context.SaveChangesAsync();
            return notif;
        }
        #endregion

        #region AtachNotificationToUserAsync

        public async Task<bool> AtachNotificationToUserAsync(UserNotification userNotification)
        {
            var context = await Context.UserNotifications.AnyAsync(i=>i.UserId==userNotification.UserId&&i.NotificationId==userNotification.NotificationId);
            if(!context)
            {
                await Context.UserNotifications.AddAsync(userNotification);
                await Context.SaveChangesAsync();
                return true;
            }
            throw new ArgumentException("Such record already exist in BookBridgeDB");
        }

        #endregion
    }
}
