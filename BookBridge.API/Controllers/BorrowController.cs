using System.Security.Claims;
using BookBridge.Application.Interfaces;
using BookBridge.Application.Models;
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


        /// <summary>
        /// Get borrowed book detail by ID
        /// </summary>
        /// <returns>Returns  a   borrow record.</returns>
        [HttpPost]
        [Route("[action]/{bookId}")]
        public async Task<Response<BorrowRecordModel>> BorrowBook([FromRoute] long bookId)
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

        [HttpPatch]
        [Route("[action]/{bookId}")]
        public async Task<Response<BorrowRecordModel>> ReturnBook([FromRoute] long bookId)
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

        [HttpPost]
        [Route("[action]")]
        public async Task<Response<IEnumerable<BorrowRecordModel>>> GetUserBorrowRecords()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Response<IEnumerable<BorrowRecordModel>>.Error(ErrorKeys.Unauthorized);
            }
            var res = await borrowService.GetUserBorrowRecordsAsync(userId);
            if (!res.Any()) return Response<IEnumerable<BorrowRecordModel>>.Error(ErrorKeys.NotFound);
            return Response<IEnumerable<BorrowRecordModel>>.Ok(res);

        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [Route(nameof(UpdateBorrowRecordsDueDate))]
        public async Task<Response<bool>> UpdateBorrowRecordsDueDate([FromBody] UpdateBorrowRecordModel updateBorrowRecordModel)
        {

            ArgumentNullException.ThrowIfNull(updateBorrowRecordModel, nameof(updateBorrowRecordModel));
            var res = await borrowService.UpdateDueDateAsync(updateBorrowRecordModel);
            return res ? Response<bool>.Ok(res)
                : Response<bool>.Error(ErrorKeys.BadRequest);


        }

        [Authorize(Roles ="ADMIN")]
        [HttpGet]
        [Route(nameof(SendReminders))]
        public async Task SendReminders()
        {
            await borrowService.SendRemindersAsync();
        }

    }
}