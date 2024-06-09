using BookBridge.Domain.Entities;

namespace BookBridge.Domain.Interfaces
{
    public interface IReviewRepo:ICrudRepo<Review,long>
    {
        Task<IEnumerable<Review>> GetBookReviewsAsync(long bookId);
        Task<IEnumerable<Review>> GetUserReviewsAsync(string userId);
    }
}
