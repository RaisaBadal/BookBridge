using BookBridge.Domain.Data;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Infrastructure.Repositories
{
    public class WishlistRepo:AbstractClass<Wishlist>,IWishlistRepo
    {
        public WishlistRepo(BookBridgeDb context) : base(context)
        {
        }

        #region CreateWishlistAsync
        public async Task<bool> CreateWishlistAsync(string userId)
        {
            var user = await Context.Users.FindAsync(userId) ??
                       throw new KeyNotFoundException($"No user found by id: {userId}");
            var wishlist = new Wishlist();
            wishlist.UserId=userId;
            await DbSet.AddAsync(wishlist);
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region AddItemToWishlistAsync

        public async Task<WishlistItem> AddItemToWishlistItemAsync(long bookId, string userId)
        {
            var user = await Context.Users.FindAsync(userId) ??
                       throw new KeyNotFoundException($"No user found by id: {userId}");
            var book = await Context.Books.FindAsync(bookId) ??
                       throw new KeyNotFoundException($"No book found by id: {bookId}");
            var wishlistItem = new WishlistItem()
            {
                BookId = bookId,
                WishlistId = await DbSet.Where(i=>i.UserId==userId).Select(i=>i.Id).FirstOrDefaultAsync()
            };
            await Context.WishlistItems.AddAsync(wishlistItem);
            Context.SaveChanges();
            return wishlistItem;
        }
        #endregion

        #region RemoveItemFromWishlistAsync

        public async Task<bool> RemoveItemFromWishlistAsync(long bookId, string userId)
        {
            var wishlist = await DbSet.FirstOrDefaultAsync(i => i.UserId == userId)
                           ?? throw new KeyNotFoundException($"No user found by id: {userId}");
            var wishlistItem =
                await Context.WishlistItems.Where(i => i.WishlistId == wishlist.Id && i.BookId==bookId).FirstOrDefaultAsync();
            if (wishlistItem == null) throw new Exception();
            Context.WishlistItems.Remove(wishlistItem);
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region GetUserWishlistAsync

        public async Task<IEnumerable<WishlistItem>> GetUserWishlistAsync(string userId)
        {
            var wishlist = await DbSet.FirstOrDefaultAsync(i => i.UserId == userId)
                           ?? throw new KeyNotFoundException($"No user found by id: {userId}");
            var wishlistItem = await Context.WishlistItems.Where(i => i.WishlistId == wishlist.Id).ToListAsync();
            return wishlistItem;
        }

        #endregion
    }
}
