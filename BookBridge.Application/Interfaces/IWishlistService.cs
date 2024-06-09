using BookBridge.Application.Models.Request;

namespace BookBridge.Application.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<ReviewModel>> GetBookReviewsAsync(long bookId);
        Task<IEnumerable<ReviewModel>> GetUserReviewsAsync(string userId);
        Task<bool> CreateWishlistAsync(string userId);
        Task<WishlistItemModel> AddItemToWishlistItemAsync(long bookId, string userId);
        Task<bool> RemoveItemFromWishlistAsync(long bookId, string userId);
        Task<IEnumerable<WishlistItemModel>> GetUserWishlistAsync(string userId);
    }
}
