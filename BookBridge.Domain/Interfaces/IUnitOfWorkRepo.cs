namespace BookBridge.Domain.Interfaces
{
    public interface IUnitOfWorkRepo:IDisposable
    {
        IAuthorRepo AuthorRepo { get; }

        IBookCategoryRepo BookCategoryRepo { get; }

        IBookRepo BookRepo { get; }

        IBorrowRecord BorrowRecord { get; }

        INotificationRepo NotificationRepo { get; }

        IReviewRepo ReviewRepo { get; }

        IWishlistRepo WishlistRepo { get; }

        Task Savechanges();

    }
}
