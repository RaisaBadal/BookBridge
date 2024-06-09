using AutoMapper;
using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.StaticFiles;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;

namespace BookBridge.Application.Services
{
    public class WishlistService(IUnitOfWorkRepo unitOfWorkRepo, IMapper autoMapper) :AbstractService(unitOfWorkRepo, autoMapper)
        , IWishlistService, IReviewService
    {
        #region GetBookReviewsAsync

        public async Task<IEnumerable<ReviewModel>> GetBookReviewsAsync(long bookId)
        {
            try
            {
                if (bookId < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                var bookReviews = await unitOfWorkRepo.ReviewRepo.GetBookReviewsAsync(bookId);
                var mapped = autoMapper.Map<IEnumerable<ReviewModel>>(bookReviews)
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

        #region GetUserReviewsAsync

        public async Task<IEnumerable<ReviewModel>> GetUserReviewsAsync(string userId)
        {
            try
            {
               ArgumentNullException.ThrowIfNull(userId);
                var bookReviews = await unitOfWorkRepo.ReviewRepo.GetUserReviewsAsync(userId);
                var mapped = autoMapper.Map<IEnumerable<ReviewModel>>(bookReviews)
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

        #region CreateWishlistAsync

        public async Task<bool> CreateWishlistAsync(string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var wishlist = await unitOfWorkRepo.WishlistRepo.CreateWishlistAsync(userId);
                return wishlist;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region AddItemToWishlistItemAsync

        public async Task<WishlistItemModel> AddItemToWishlistItemAsync(long bookId, string userId)
        {
            try
            {
                if (bookId < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                ArgumentNullException.ThrowIfNull(userId);
                var wishlist = await unitOfWorkRepo.WishlistRepo.AddItemToWishlistItemAsync(bookId, userId);
                var mapped = autoMapper.Map<WishlistItemModel>(wishlist)
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

        #region RemoveItemFromWishlistAsync

        public async Task<bool> RemoveItemFromWishlistAsync(long bookId, string userId)
        {
            try
            {
                if (bookId < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                ArgumentNullException.ThrowIfNull(userId);
                var remove = await unitOfWorkRepo.WishlistRepo.RemoveItemFromWishlistAsync(bookId, userId);
                return remove;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region GetUserWishlistAsync
        public async Task<IEnumerable<WishlistItemModel>> GetUserWishlistAsync(string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId);
                var wishlist = await unitOfWorkRepo.WishlistRepo.GetUserWishlistAsync(userId);
                var mapped = autoMapper.Map<IEnumerable<WishlistItemModel>>(wishlist)
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

        #region AddReviewAsync

        public async Task<long> AddAsync(ReviewModel entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity, ErrorKeys.ArgumentNull);
                var mapped = autoMapper.Map<Review>(entity)
                                 ?? throw new ArgumentException(ErrorKeys.Mapped);
                var review = await unitOfWorkRepo.ReviewRepo.AddAsync(mapped);
                return review;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region RemoveReviewAsync

        public async Task<bool> RemoveAsync(long id)
        {
            try
            {
                if (id < -1) throw new ArgumentException(ErrorKeys.BadRequest);
                var res = await unitOfWorkRepo.ReviewRepo.RemoveAsync(id);
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region UpdateReviewAsync
        public async Task<bool> UpdateAsync(long id, ReviewModel entity)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                var mapped = autoMapper.Map<Review>(entity)
                             ?? throw new ArgumentException(ErrorKeys.Mapped);
                var updateReview = await unitOfWorkRepo.ReviewRepo.UpdateAsync(id, mapped);
                return updateReview;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region SoftDeleteReviewAsync

        public async Task<bool> SoftDeleteAsync(long id)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                var review = await unitOfWorkRepo.ReviewRepo.SoftDeleteAsync(id);
                return review;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region GetAllReviewAsync

        public async Task<IEnumerable<ReviewModel>> GetAllAsync()
        {
            try
            {
                var review = await UnitOfWorkRepo.ReviewRepo.GetAllAsync();
                var mappedReviewModel = autoMapper.Map<IEnumerable<ReviewModel>>(review);
                return mappedReviewModel;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region GetByIdReviewAsync
        public async Task<ReviewModel> GetByIdAsync(long id)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                var review = await unitOfWorkRepo.ReviewRepo.GetByIdAsync(id);
                ArgumentNullException.ThrowIfNull(review);
                var mapped = autoMapper.Map<ReviewModel>(review)
                             ?? throw new ArgumentNullException(ErrorKeys.ArgumentNull);
                return mapped;
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
