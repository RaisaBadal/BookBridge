using BookBridge.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Domain.Data
{
    public class BookBridgeDb(DbContextOptions<BookBridgeDb> options) : IdentityDbContext<User>(options)
    {
        public virtual DbSet<Author>Authors { get; set; }
        public virtual DbSet<Book>Books { get; set; }
        public virtual DbSet<BookCategory>BookCategories { get; set; }
        public virtual DbSet<BorrowRecord>BorrowRecords { get; set; }
        public virtual DbSet<Review>Reviews { get; set; }
        public virtual DbSet<Wishlist>Wishlists { get; set; }
        public virtual DbSet<WishlistItem> WishlistItems { get; set; }
        public virtual DbSet<Notification>Notifications { get; set; }
        public virtual DbSet<UserNotification> UserNotifications { get; set; }
    }
}
