using AutoMapper;
using BookBridge.Application.Exceptions;
using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.StaticFiles;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;
using BookBridge.Persistance.SMTPService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Application.Services
{
    public class NotificationService:AbstractService,INotificationService
    {
        private readonly UserManager<User> userManager;
        private readonly SmtpService smtpService;

        public NotificationService(IUnitOfWorkRepo unitOfWorkRepo, IMapper autoMapper, UserManager<User> userManager, SmtpService smtpService)
            : base(unitOfWorkRepo, autoMapper)
        { 
            this.userManager = userManager;
            this.smtpService = smtpService;
        }

        #region CreateNotificationAsync
        public async Task<NotificationModel> CreateNotificationAsync(string message)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(message);
                var notification = await UnitOfWorkRepo.NotificationRepo.CreateNotificationAsync(message);
                if(notification != null)
                {
                    var mapped = AutoMapper.Map<NotificationModel>(notification)
                        ?? throw new ArgumentNullException(ErrorKeys.Mapped);
                    return mapped;
                }
                throw new ArgumentNullException(ErrorKeys.UnSuccessFullInsert);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion

        #region DeleteNotificationAsync
        public async Task DeleteNotificationAsync(long notificationId)
        {
            try
            {
                if (notificationId < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                await UnitOfWorkRepo.NotificationRepo.DeleteNotificationAsync(notificationId);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetAllNotificationAsync

        public async Task<IEnumerable<UserNotificationModel>> GetAllNotificationAsync()
        {
            try
            {
                var notification=await UnitOfWorkRepo.NotificationRepo.GetAllNotificationAsync();
                var mapped = AutoMapper.Map<IEnumerable<UserNotificationModel>>(notification)
                    ?? throw new ArgumentException(ErrorKeys.Mapped);
                return mapped;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetNotificationByIdAsync
        public async Task<NotificationModel> GetNotificationByIdAsync(long notificationId)
        {
            try
            {
                if(notificationId < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                var notification=await UnitOfWorkRepo.NotificationRepo.GetNotificationByIdAsync(notificationId);
                if(notification == null) throw new InvalidOperationException(ErrorKeys.BadRequest);
                var mapped = AutoMapper.Map<NotificationModel>(notification)
                    ?? throw new InvalidOperationException(ErrorKeys.Mapped);
                return mapped;

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetUserNotificationsAsync
        public async Task<IEnumerable<NotificationModel>> GetUserNotificationsAsync(string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId);
                var user=await userManager.FindByIdAsync(userId);
                if (user == null) throw new UserNotFoundException("There is no current user!");
                var notification = await GetAllNotificationAsync();
                var userNotification = notification.Where(i => i.UserId == userId).ToList();
                if (!userNotification.Any()) throw new NotificationNotFoundException($"No notification found for user: {userId}");
                var mapped = AutoMapper.Map<IEnumerable<NotificationModel>>(userNotification)
                    ?? throw new InvalidOperationException(ErrorKeys.Mapped);
                return mapped;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region MarkNotificationAsSentAsync
        public async Task<bool> MarkNotificationAsSentAsync(long notificationId)
        {
            try
            {
                if (notificationId < 0) throw new GeneralException(ErrorKeys.BadRequest);
                var notificationList = await UnitOfWorkRepo.NotificationRepo.GetAllNotificationAsync();
                var filterNotificationUser= notificationList.Where(i=>i.NotificationId == notificationId).ToList();
                if (!filterNotificationUser.Any()) throw new NotificationNotFoundException($"No notification found for id: {notificationId}");
                foreach (var item in filterNotificationUser)
                {
                    if (item.SentDate != null)
                    {
                        item.IsSent = true;
                    }
                   await  UnitOfWorkRepo.Savechanges();
                }
                return true;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                throw;
            }
        }
        #endregion

        #region SendNotificationToAllUsersAsync
        public async Task<bool> SendNotificationToAllUsersAsync(long notificationId)
        {
            try
            {
                if (notificationId < 0) throw new GeneralException(ErrorKeys.BadRequest);
                string name= " ";
                string surname= " ";
                var notification=await GetNotificationByIdAsync(notificationId);
                if (notification == null) throw new NotificationNotFoundException($"No notification found by id: {notificationId}");

                var body = $@"<!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 20px;
                    background-color: #f4f4f9;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border: 1px solid #dddddd;
                    border-radius: 4px;
                }}
                .header {{
                    background-color: #007BFF;
                    color: #ffffff;
                    padding: 10px;
                    border-radius: 4px 4px 0 0;
                    text-align: center;
                }}
                .content {{
                    padding: 20px;
                }}
                .content p {{
                    margin: 0 0 10px;
                }}
                .footer {{
                    padding: 10px;
                    text-align: center;
                    font-size: 12px;
                    color: #aaaaaa;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header"">
                    <h1>Library Notification</h1>
                </div>
                <div class=""content"">
                    <p>Dear {name+" "+ surname},</p>
                    <p>{notification.Message}</p>
                    <p>If you have any questions, feel free to contact us.</p>
                    <p>Best regards,</p>
                    <p>The BookBridge Team</p>
                </div>
            <div class=""footer"">
                <p>&copy; 2024 The Library. All rights reserved.</p>
            </div>
        </div>
    </body>
</html>";
                var users = await userManager.Users.ToListAsync();
                foreach (var item in users)
                {
                    name=item.Name;
                    surname=item.Surname;
                    smtpService.SendMessage(item.Email, "Library Notification", body);
                    var notificationUser = new UserNotificationModel()
                    {
                        SentDate = DateTime.Now,
                        NotificationId = notificationId,
                        UserId = item.Id
                    };
                    var mapped = AutoMapper.Map<UserNotification>(notificationUser);
                    await UnitOfWorkRepo.NotificationRepo.AtachNotificationToUserAsync(mapped);
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region SendNotificationToUserAsync
        public async Task<bool> SendNotificationToUserAsync(long notificationId, string userId)
        {
            try
            {
                if(notificationId< 0) throw new GeneralException( ErrorKeys.BadRequest);
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var user=await userManager.FindByIdAsync(userId);
                if (user == null) throw new UserNotFoundException($"No user found by id: {userId}");
                var notification=await GetNotificationByIdAsync(notificationId);
                if (notification == null) throw new NotificationNotFoundException($"No notification found by id: {notificationId}");

                var body = $@"<!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 20px;
                    background-color: #f4f4f9;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border: 1px solid #dddddd;
                    border-radius: 4px;
                }}
                .header {{
                    background-color: #007BFF;
                    color: #ffffff;
                    padding: 10px;
                    border-radius: 4px 4px 0 0;
                    text-align: center;
                }}
                .content {{
                    padding: 20px;
                }}
                .content p {{
                    margin: 0 0 10px;
                }}
                .footer {{
                    padding: 10px;
                    text-align: center;
                    font-size: 12px;
                    color: #aaaaaa;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header"">
                    <h1>Library Notification</h1>
                </div>
                <div class=""content"">
                    <p>Dear {user.Name+" "+user.Surname},</p>
                    <p>{notification.Message}</p>
                    <p>If you have any questions, feel free to contact us.</p>
                    <p>Best regards,</p>
                    <p>The BookBridge Team</p>
                </div>
            <div class=""footer"">
                <p>&copy; 2024 The Library. All rights reserved.</p>
            </div>
        </div>
    </body>
</html>";

                smtpService.SendMessage(user.Email, "BookBridge Notification", body);
                var userNotification = new UserNotificationModel()
                {
                    SentDate = DateTime.Now,
                    NotificationId = notificationId,
                    UserId = user.Id
                };
                var mapped=AutoMapper.Map<UserNotification>(userNotification);
                var res=await UnitOfWorkRepo.NotificationRepo.AtachNotificationToUserAsync(mapped);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region SendNotificationToUsersAsync
        public async Task<bool> SendNotificationToUsersAsync(long notificationId, List<string> usersIds)
        {
            try
            {
                if(notificationId<0) throw new GeneralException(ErrorKeys.BadRequest);
                var notification = await GetNotificationByIdAsync(notificationId);
                if(notification==null) throw new NotificationNotFoundException($"No notification found by id: {notificationId}");
                string name = " ";
                string surname = " ";
                var body = $@"<!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 20px;
                    background-color: #f4f4f9;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border: 1px solid #dddddd;
                    border-radius: 4px;
                }}
                .header {{
                    background-color: #007BFF;
                    color: #ffffff;
                    padding: 10px;
                    border-radius: 4px 4px 0 0;
                    text-align: center;
                }}
                .content {{
                    padding: 20px;
                }}
                .content p {{
                    margin: 0 0 10px;
                }}
                .footer {{
                    padding: 10px;
                    text-align: center;
                    font-size: 12px;
                    color: #aaaaaa;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header"">
                    <h1>Library Notification</h1>
                </div>
                <div class=""content"">
                    <p>Dear {name + " " + surname},</p>
                    <p>{notification.Message}</p>
                    <p>If you have any questions, feel free to contact us.</p>
                    <p>Best regards,</p>
                    <p>The BookBridge Team</p>
                </div>
            <div class=""footer"">
                <p>&copy; 2024 The Library. All rights reserved.</p>
            </div>
        </div>
    </body>
</html>";
                foreach (var item in usersIds)
                {
                    var user = await userManager.FindByIdAsync(item);
                    if (user is { Email: not null, Name: not null, Surname: not null })
                    {
                        name = user.Name;
                        surname = user.Surname;
                        smtpService.SendMessage(user.Email, $"BookBridge Notification {DateTime.Now.ToShortTimeString()}", body);
                        var notificationUser = new UserNotificationModel
                        {
                            SentDate = DateTime.Now,
                            NotificationId = notificationId,
                            UserId = user.Id
                        };
                        var mapped = AutoMapper.Map<UserNotification>(notificationUser);
                        await UnitOfWorkRepo.NotificationRepo.AtachNotificationToUserAsync(mapped);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region UpdateNotificationAsync
        public async Task<NotificationModel> UpdateNotificationAsync(long id, NotificationModel notification)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(notification, nameof(notification));
                if (id < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                var map=AutoMapper.Map<Notification>(notification);
                var res=await UnitOfWorkRepo.NotificationRepo.UpdateNotificationAsync(id,map);
                if (res == null) throw new GeneralException(ErrorKeys.BadRequest);
                var mapped = AutoMapper.Map<NotificationModel>(notification) 
                    ?? throw new InvalidOperationException(ErrorKeys.Mapped);
                return mapped;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
    }
}
