using BookBridge.Domain.Data;
using BookBridge.Domain.Interfaces;

namespace BookBridge.Infrastructure.Repositories
{
    public class UnitOfWork(BookBridgeDb bookBridgeDb) : IUnitOfWorkRepo
    {
        public IAuthorRepo AuthorRepo => new AuthorRepo(bookBridgeDb);
        public IBookCategoryRepo BookCategoryRepo => new BookCategoryRepo(bookBridgeDb);
        public IBookRepo BookRepo => new BookRepo(bookBridgeDb);
        public IBorrowRecord BorrowRecord => new BorrowRecordRepo(bookBridgeDb);
        public INotificationRepo NotificationRepo => new NotificationRepo(bookBridgeDb);
        public IReviewRepo ReviewRepo => new ReviewRepo(bookBridgeDb);
        public IWishlistRepo WishlistRepo=>new WishlistRepo(bookBridgeDb);

        public void Dispose()
        {
            bookBridgeDb.Dispose();
        }
    }
}
