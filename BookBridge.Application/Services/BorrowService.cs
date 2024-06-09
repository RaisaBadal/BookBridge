using AutoMapper;
using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.StaticFiles;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;
using BookBridge.Persistance.SMTPService;
using Microsoft.AspNetCore.Identity;

namespace BookBridge.Application.Services
{
    public class BorrowService : AbstractService, IBorrowService
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;
        private readonly IMapper autoMapper;
        private readonly UserManager<User> userManager;
        private readonly SmtpService smtpService;

        public BorrowService(IUnitOfWorkRepo unitOfWorkRepo, IMapper autoMapper, UserManager<User> userManager, SmtpService smtpService)
            : base(unitOfWorkRepo, autoMapper)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
            this.autoMapper = autoMapper;
            this.userManager = userManager;
            this.smtpService = smtpService;
        }

        #region BorrowBookAsync
        public async Task<BorrowRecordModel> BorrowBookAsync(long bookId, string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId);
                if (bookId < 0) throw new ArgumentNullException(ErrorKeys.ArgumentNull);
                var borrow = await unitOfWorkRepo.BorrowRecord.BorrowBookAsync(bookId, userId)
                             ?? throw new ArgumentException(ErrorKeys.BadRequest);
                var mapped = autoMapper.Map<BorrowRecordModel>(borrow)
                             ?? throw new ArgumentException(ErrorKeys.Mapped);
                var user = await userManager.FindByIdAsync(userId);
                if (user == null) throw new UnauthorizedAccessException();
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
            <h1>Library Book Borrowing Information</h1>
        </div>
        <div class=""content"">
            <p>Dear {user.UserName},</p>
            <p>Thank you for borrowing a book from our library. Here are the details of your borrowed book:</p>
            <ul>
                <li><strong>Book ID:</strong> {bookId}</li>
                <li><strong>Borrow Date:</strong> {DateTime.Now}</li>
                <li><strong>Due Date:</strong> {DateTime.Now.AddDays(5)}</li>
            </ul>
            <p>Please make sure to return the book by the due date to avoid any late fees.</p>
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
                smtpService.SendMessage(user.Email, $"Borrow book reminder {DateTime.Now.ToShortTimeString()}", body);
                return mapped;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region ReturnBookAsync
        public async Task<BorrowRecordModel> ReturnBookAsync(long bookId, string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId);
                if (bookId < 0) throw new ArgumentNullException(ErrorKeys.ArgumentNull);
                var returned = await unitOfWorkRepo.BorrowRecord.ReturnBookAsync(bookId, userId)
                               ?? throw new ArgumentException(ErrorKeys.BadRequest);
                var mapped = autoMapper.Map<BorrowRecordModel>(returned)
                             ?? throw new ArgumentException(ErrorKeys.Mapped);
                return mapped;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region CreateNotificationAsync
        public async Task<NotificationModel> CreateNotificationAsync(string userId, string message)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(message);
                ArgumentNullException.ThrowIfNull(userId);

                var notification = await unitOfWorkRepo.NotificationRepo.CreateNotificationAsync(userId, message)
                                    ?? throw new ArgumentException(ErrorKeys.BadRequest);

                var mapped = autoMapper.Map<NotificationModel>(notification)
                                ?? throw new ArgumentException(ErrorKeys.Mapped);

                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
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
                                    <p>Dear {user.UserName},</p>
                                    <p>{message}</p>
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

                    smtpService.SendMessage(user.Email, "Library Notification", body);
                }

                return mapped;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region CreateNotificationsAsync
        public async Task<IEnumerable<NotificationModel>> CreateNotificationsAsync(IEnumerable<string> userIds, string message)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userIds);
                ArgumentNullException.ThrowIfNull(message);

                var notifications = new List<NotificationModel>();

                foreach (var userId in userIds)
                {
                    var notification = await CreateNotificationAsync(userId, message);
                    notifications.Add(notification);
                }

                return notifications;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region MarkNotificationAsSentAsync
        public async Task<NotificationModel> MarkNotificationAsSentAsync(long notificationId)
        {
            try
            {
                if (notificationId < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);

                var notification = await unitOfWorkRepo.NotificationRepo.MarkNotificationAsSentAsync(notificationId)
                                    ?? throw new KeyNotFoundException($"Notification not found by id: {notificationId}");

                var mapped = autoMapper.Map<NotificationModel>(notification)
                            ?? throw new ArgumentException(ErrorKeys.Mapped);

                return mapped;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
               var notification = await unitOfWorkRepo.NotificationRepo.GetUserNotificationsAsync(userId);
               var mapped = autoMapper.Map<IEnumerable<NotificationModel>>(notification)
                            ?? throw new ArgumentException(ErrorKeys.Mapped);
               return mapped;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region DeleteNotificationAsync

        public async Task DeleteNotificationAsync(long notificationId)
        {
            try
            {
                if (notificationId < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                await unitOfWorkRepo.NotificationRepo.DeleteNotificationAsync(notificationId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion
    }
}
