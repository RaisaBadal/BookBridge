using AutoMapper;
using BookBridge.Application.Exceptions;
using BookBridge.Application.Interfaces;
using BookBridge.Application.Models;
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
            <h1>Library Book Return Information</h1>
        </div>
        <div class=""content"">
            <p>Dear {user.UserName},</p>
            <p>Thank you for returning the book to our library. Here are the details of your returned book:</p>
            <ul>
                <li><strong>Book ID:</strong> {bookId}</li>
                <li><strong>Return Date:</strong> {DateTime.Now}</li>
            </ul>
            <p>We hope you enjoyed reading the book. If you have any questions, feel free to contact us.</p>
            <p>Best regards,</p>
            <p>The BookBridge Team</p>
        </div>
        <div class=""footer"">
            <p>&copy; 2024 The Library. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

                smtpService.SendMessage(user.Email, $"Book Return Confirmation {DateTime.Now.ToShortTimeString()}", body);
                return mapped;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region GetUserBorrowRecordsAsync
        public async Task<IEnumerable<BorrowRecordModel>> GetUserBorrowRecordsAsync(string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var user=await userManager.FindByIdAsync(userId);
                if(user == null) throw new UserNotFoundException(nameof(user));
                var borrowRecord = await unitOfWorkRepo.BorrowRecord.ReturnAllActiveBorrowRecordAsync();
                if (borrowRecord == null) throw new NotFoundException("No borrow record found in DB");
                var filteredRecord=borrowRecord.Where(i=>i.UserId == userId);
                if (!filteredRecord.Any()) throw new NotFoundException($"No active borrow record is found for user {userId}");
                var mapped = autoMapper.Map<IEnumerable<BorrowRecordModel>>(filteredRecord)
                    ?? throw new GeneralException(ErrorKeys.Mapped);
                return mapped;
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region UpdateDueDateAsync

        public async Task<bool> UpdateDueDateAsync(UpdateBorrowRecordModel updateBorrowRecordModel)
        {
            try
            {
                if(updateBorrowRecordModel.bookId < 0)throw new ArgumentNullException(nameof(updateBorrowRecordModel.bookId));
                ArgumentNullException.ThrowIfNull(updateBorrowRecordModel.userId, nameof(updateBorrowRecordModel.userId));
                var record = await unitOfWorkRepo.BorrowRecord.ReturnAllActiveBorrowRecordAsync();
                if(!record.Any()) throw new NotFoundException(ErrorKeys.NotFound);
                var filterRecord=record.Where(i=>i.UserId== updateBorrowRecordModel.userId 
                && i.BookId== updateBorrowRecordModel.bookId);
                if(!filterRecord.Any())throw new NotFoundException(ErrorKeys.NotFound);
                foreach (var item in filterRecord)
                {
                    item.DueDate = updateBorrowRecordModel.DueDate;
                    await unitOfWorkRepo.Savechanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion

        #region SendRemindersAsync
        public async Task SendRemindersAsync()
        {
            try
            {
                var borrowRecords = await unitOfWorkRepo.BorrowRecord.ReturnAllActiveBorrowRecordAsync();
                var filterBorrowRecords = borrowRecords.Where(i => i.DueDate < DateTime.Now.AddDays(1)).ToList();
                if (!filterBorrowRecords.Any()) throw new NotFoundException(ErrorKeys.NoBook);

                foreach (var borrowRecord in filterBorrowRecords)
                {
                    var user = await userManager.FindByIdAsync(borrowRecord.UserId);
                    if (user == null) continue;

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
            <h1>Library Book Return Reminder</h1>
        </div>
        <div class=""content"">
            <p>Dear {user.UserName},</p>
            <p>This is a friendly reminder that your borrowed book is due to be returned soon. Here are the details of your borrowed book:</p>
            <ul>
                <li><strong>Book ID:</strong> {borrowRecord.BookId}</li>
                <li><strong>Borrow Date:</strong> {borrowRecord.BorrowDate}</li>
                <li><strong>Due Date:</strong> {borrowRecord.DueDate}</li>
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

                    smtpService.SendMessage(user.Email, "Library Book Return Reminder", body);
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }
        #endregion
    }
}
