using System.Security.Claims;
using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.response;
using BookBridge.Application.StaticFiles;
using BookBridge.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BookBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BorrowController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly IBorrowService borrowService;
        private readonly UserManager<User> userManager;

        public BorrowController(IMemoryCache memoryCache, IBorrowService borrowService, UserManager<User> userManager)
        {
            this.memoryCache = memoryCache;
            this.borrowService = borrowService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Book: {bookId}/[action]")]
        public async Task<Response<BorrowRecordModel>> BorrowBook([FromRoute] long bookId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Response<BorrowRecordModel>.Error(ErrorKeys.Unauthorized);
                }

                var res = await borrowService.BorrowBookAsync(bookId, userId);
                return res == null ? Response<BorrowRecordModel>.Error(ErrorKeys.NotFound)
                    : Response<BorrowRecordModel>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<BorrowRecordModel>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPatch]
        [Route("Book: {bookId}/[action]")]
        public async Task<Response<BorrowRecordModel>> ReturnBook([FromRoute] long bookId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Response<BorrowRecordModel>.Error(ErrorKeys.Unauthorized);
                }
                var res = await borrowService.ReturnBookAsync(bookId, userId);
                return res == null ? Response<BorrowRecordModel>.Error(ErrorKeys.NotFound)
                    : Response<BorrowRecordModel>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<BorrowRecordModel>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("User: {userId}/[action]")]
        public async Task<Response<NotificationModel>> CreateNotification([FromRoute] string userId, [FromHeader] string message)
        {
            try
            {
                var res = await borrowService.CreateNotificationAsync(userId, message);
                return res == null ? Response<NotificationModel>.Error(ErrorKeys.BadRequest)
                    : Response<NotificationModel>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<NotificationModel>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Users/[action]")]
        public async Task<Response<IEnumerable<NotificationModel>>> CreateNotifications([FromBody] List<string> userIds, [FromHeader] string message)
        {
            try
            {
                var res = await borrowService.CreateNotificationsAsync(userIds, message);
                return res == null ? Response<IEnumerable<NotificationModel>>.Error(ErrorKeys.BadRequest)
                    : Response<IEnumerable<NotificationModel>>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<NotificationModel>>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPatch]
        [Route("{notificationId}/[action]")]
        public async Task<Response<NotificationModel>> MarkNotificationAsSentAsync([FromRoute] long notificationId)
        {
            try
            {
                var res = await borrowService.MarkNotificationAsSentAsync(notificationId);
                return res == null ? Response<NotificationModel>.Error(ErrorKeys.BadRequest)
                    : Response<NotificationModel>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<NotificationModel>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{userId}/[action]")]
        public async Task<Response<IEnumerable<NotificationModel>>> UserNotifications([FromRoute] string userId)
        {
            try
            {
                var cacheKey = $"{userId}UserNotification";
                if (memoryCache.TryGetValue(cacheKey, out IEnumerable<NotificationModel?> model))
                {
                    if (model is not null) return Response<IEnumerable<NotificationModel>>.Ok(model);
                }

                var res = await borrowService.GetUserNotificationsAsync(userId);
                if (res == null)
                {
                    return Response<IEnumerable<NotificationModel>>.Error(ErrorKeys.NotFound);
                }

                memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
                return Response<IEnumerable<NotificationModel>>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<NotificationModel>>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("{notificationId}/[action]")]
        public async Task<Response<bool>> DeleteNotification([FromRoute] long notificationId)
        {
            try
            {
                await borrowService.DeleteNotificationAsync(notificationId);
                return Response<bool>.Ok(true);
            }
            catch (Exception e)
            {
                return Response<bool>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }
    }
}