using BookBridge.Domain.Entities;

namespace BookBridge.Domain.Interfaces
{
    public interface IWishlistRepo
    {
        Task<bool> CreateWishlistAsync(string userId);
        Task<WishlistItem> AddItemToWishlistItemAsync(long bookId, string userId);
        Task<bool> RemoveItemFromWishlistAsync(long bookId, string userId);
        Task <IEnumerable<WishlistItem>> GetUserWishlistAsync(string userId);
    }
}
