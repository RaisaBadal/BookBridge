using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using BookBridge.Application.StaticFiles;
using BookBridge.Application.Services;

namespace BookBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController(IWishlistService wishlistService, IReviewService reviewService, IMemoryCache memoryCache) : ControllerBase
    {
        private readonly IWishlistService wishlistService = wishlistService;
        private readonly IReviewService reviewService = reviewService;
        private readonly IMemoryCache memoryCache = memoryCache;

        //ReviewEndpoint
        [HttpPost]
        [Route("[action]/{bookId:long}")]
        public async Task<Response<IEnumerable<ReviewModel>>> GetBookReviews([FromRoute]long bookId)
        {
            try
            {
                var cacheKey = $"BookReview{bookId}";
                if (memoryCache.TryGetValue(cacheKey, out IEnumerable<ReviewModel?> model))
                {
                    if(model!=null) return Response<IEnumerable<ReviewModel>>.Ok(model);
                }

                var res = await wishlistService.GetBookReviewsAsync(bookId);
                if (res==null) return Response< IEnumerable < ReviewModel >>.Error(ErrorKeys.NotFound);
                memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
                return Response<IEnumerable<ReviewModel>>.Ok(res);
            }
            catch (Exception e)
            {
               return Response < IEnumerable < ReviewModel >>.Error(e.Message,e.StackTrace,ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]/{userId:alpha}")]
        public async Task<Response<IEnumerable<ReviewModel>>> GetUserReviews(string userId)
        {
            try
            {
                var cacheKey = $"UserReview{userId}";
                if (memoryCache.TryGetValue(cacheKey, out IEnumerable<ReviewModel?> model))
                {
                    if (model!=null) return Response<IEnumerable<ReviewModel>>.Ok(model);
                }

                var res = await wishlistService.GetUserReviewsAsync(userId);
                if(res==null) return Response<IEnumerable<ReviewModel>>.Error(ErrorKeys.NotFound);
                memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
                return Response<IEnumerable<ReviewModel>>.Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Route(nameof(InsertReview))]
        public async Task<Response<long>> InsertReview([FromBody] ReviewModel entity)
        {
            try
            {
                if (!ModelState.IsValid || entity is null) return Response<long>.Error(ErrorKeys.BadRequest);
                var res = await reviewService.AddAsync(entity);
                return res != -1 ? Response<long>.Ok(res) : Response<long>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception e)
            {
                return Response<long>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> RemoveReview([FromRoute] long id)
        {
            try
            {
                var res = await reviewService.RemoveAsync(id);
                return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception e)
            {
                return Response<bool>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> UpdateReview([FromRoute] long id, [FromBody] ReviewModel entity)
        {
            try
            {
                if (!ModelState.IsValid || entity is null) return Response<bool>.Error(ErrorKeys.BadRequest);
                var res = await reviewService.UpdateAsync(id, entity);
                return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception e)
            {
                return Response<bool>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> SoftDeleteReview([FromRoute] long id)
        {
            try
            {
                var res = await reviewService.SoftDeleteAsync(id);
                return Response<bool>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<bool>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<ReviewModel>>> AllReview()
        {
            try
            {
                const string cacheKey = "GetAllReview";
                if (memoryCache.TryGetValue(cacheKey, out IEnumerable<ReviewModel>? cachedData))
                {
                    if (cachedData != null) return Response<IEnumerable<ReviewModel>>.Ok(cachedData);
                }
                else
                {
                    var res = await reviewService.GetAllAsync();
                    if (!res.Any())
                    {
                        return Response<IEnumerable<ReviewModel>>.Error(ErrorKeys.BadRequest);
                    }

                    memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(30));
                    return Response<IEnumerable<ReviewModel>>.Ok(res);
                }
                return Response<IEnumerable<ReviewModel>>.Error(ErrorKeys.BadRequest);

            }
            catch (Exception e)
            {
                return Response<IEnumerable<ReviewModel>>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<Response<ReviewModel>> GetByIdReview([FromRoute] long id)
        {
            try
            {
                var cacheKey = $"BookById {id}";
                if (memoryCache.TryGetValue(cacheKey, out ReviewModel? model))
                {
                    if (model != null) return Response<ReviewModel>.Ok(model);
                }
                var res = await reviewService.GetByIdAsync(id);
                if (res is null) return Response<ReviewModel>.Error(ErrorKeys.BadRequest);
                memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(30));
                return Response<ReviewModel>.Ok(res);

            }
            catch (Exception e)
            {
                return Response<ReviewModel>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        //WishlistEndpoint
        [HttpPost]
        [Route("[action]/{userId}")]
        public async Task<Response<bool>> CreateWishlist([FromRoute] string userId)
        {
            try
            {
                var res = await wishlistService.CreateWishlistAsync(userId);
                return Response<bool>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<bool>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]/{bookId}/{userId}")]
        public async Task<Response<WishlistItemModel>> AddItemToWishlistItem([FromRoute]long bookId,[FromRoute] string userId)
        {
            try
            {
                var res = await wishlistService.AddItemToWishlistItemAsync(bookId, userId);
                return res == null ? Response<WishlistItemModel>.Error(ErrorKeys.NotFound) 
                    : Response<WishlistItemModel>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<WishlistItemModel>.Error(e.Message,e.StackTrace,ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]/{bookId}/{userId}")]
        public async Task<Response<bool>> RemoveItemFromWishlist([FromRoute]long bookId, [FromRoute]string userId)
        {
            try
            {
                var res = await wishlistService.RemoveItemFromWishlistAsync(bookId, userId);
                return Response<bool>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<bool>.Error(e.Message,e.StackTrace,ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]/{userId}")]
        public async Task<Response<IEnumerable<WishlistItemModel>>> GetUserWishlist([FromRoute] string userId)
        {
            try
            {
                var res = await wishlistService.GetUserWishlistAsync(userId);
                return !res.Any()
                    ? Response<IEnumerable<WishlistItemModel>>.Error(ErrorKeys.NotFound)
                    : Response<IEnumerable<WishlistItemModel>>.Ok(res);
            }
            catch (Exception e)
            {
               return Response<IEnumerable<WishlistItemModel>>.Error(e.Message,e.StackTrace,ErrorKeys.InternalServerError);
            }
        }
    }   
}
