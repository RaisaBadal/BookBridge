using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.response;
using BookBridge.Application.StaticFiles;
using BookBridge.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace BookBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly INotificationService notificationService;
        private readonly UserManager<User> userManager;

        public NotificationController(IMemoryCache memoryCache, INotificationService notificationService, UserManager<User> userManager)
        {
            this.memoryCache = memoryCache;
            this.notificationService = notificationService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route(nameof(CreateNotification))]
        [Authorize(Roles ="ADMIN")]
        public async Task<Response<NotificationModel>> CreateNotification(string message)
        {
            try
            {
                if (!ModelState.IsValid) return Response<NotificationModel>.Error(ErrorKeys.BadRequest);
                var notification = await notificationService.CreateNotificationAsync(message);
                if (notification == null) return Response<NotificationModel>.Error(ErrorKeys.BadRequest);
                return Response<NotificationModel>.Ok(notification);
            }
            catch (Exception ex)
            {
               return Response<NotificationModel>.Error(ex.Message, ex.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("[action]/{notificationId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task DeleteNotification([FromRoute] long notificationId)
        {
            await notificationService.DeleteNotificationAsync(notificationId);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<Response<NotificationModel>> UpdateNotification([FromRoute]long id, NotificationModel notification)
        {
            try
            {
                if (!ModelState.IsValid) return Response<NotificationModel>.Error(ErrorKeys.BadRequest);
                var res= await notificationService.UpdateNotificationAsync(id, notification);
                if(res == null) return Response<NotificationModel>.Error(ErrorKeys.BadRequest);
                return Response<NotificationModel>.Ok(res);
            }
            catch (Exception ex)
            {
                return Response<NotificationModel>.Error(ex.Message, ex.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route(nameof(GetAllUserNotification))]
        [Authorize(Roles = "ADMIN")]
        public async Task<Response<IEnumerable<UserNotificationModel>>> GetAllUserNotification()
        {
            try
            {
                var res = await notificationService.GetAllNotificationAsync();
                if(!res.Any()) return Response<IEnumerable<UserNotificationModel>>.Error(ErrorKeys.BadRequest);
                return Response<IEnumerable<UserNotificationModel>>.Ok(res);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<UserNotificationModel>>.Error(ex.Message,ex.StackTrace,ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]/{notificationId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<Response<NotificationModel>> GetNotificationById([FromRoute]long notificationId)
        {
            try
            {
                var res=await notificationService.GetNotificationByIdAsync(notificationId);
                if(res==null) return Response<NotificationModel>.Error(ErrorKeys.BadRequest);
                return Response<NotificationModel>.Ok(res);
            }
            catch (Exception ex)
            {
                return Response<NotificationModel>.Error(ex.Message, ex.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Response<IEnumerable<NotificationModel>>> GetUserNotifications()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Response<IEnumerable<NotificationModel>>.Error(ErrorKeys.Unauthorized);
                }
                var res= await notificationService.GetUserNotificationsAsync(userId);
                if(!res.Any()) return Response<IEnumerable<NotificationModel>>.Error(ErrorKeys.BadRequest);
                return Response<IEnumerable<NotificationModel>>.Ok(res);
            }
            catch (Exception ex)
            {
               return Response<IEnumerable<NotificationModel>>.Error(ex.Message,ex.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]/{notificationId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<Response<bool>> MarkNotificationAsSent([FromRoute]long notificationId)
        {
            try
            {
                var res=await notificationService.MarkNotificationAsSentAsync(notificationId);
                return Response<bool>.Ok(res);
            }
            catch (Exception ex)
            {
                return Response<bool>.Error(ex.Message,ex.StackTrace,ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]/{notificationId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<Response<bool>> SendNotificationToAllUsers([FromRoute]long notificationId)
        {
            try
            {
                var res=await notificationService.SendNotificationToAllUsersAsync(notificationId);
                return res ? Response<bool>.Ok(res)
                    : Response<bool>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                return Response<bool>.Error(exp.Message, exp.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]/{notificationId}/{userId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<Response<bool>> SendNotificationToUser([FromRoute]long notificationId, [FromRoute]string userId)
        {
            try
            {
                var res = await notificationService.SendNotificationToUserAsync(notificationId, userId);
                return res ? Response<bool>.Ok(res)
                   : Response<bool>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception ex)
            {
                return Response<bool>.Error(ex.Message, ex.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]/{notificationId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<Response<bool>> SendNotificationToUsers([FromRoute]long notificationId, [FromBody]List<string> usersIds)
        {
            try
            {
                var res = await notificationService.SendNotificationToUsersAsync(notificationId, usersIds);
                return res ? Response<bool>.Ok(res)
                   : Response<bool>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception ex)
            {
                return Response<bool>.Error(ex.Message, ex.StackTrace, ErrorKeys.InternalServerError);
            }
        }


    }
}
