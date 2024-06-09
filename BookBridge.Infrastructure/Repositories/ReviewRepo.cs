using BookBridge.Domain.Data;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Infrastructure.Repositories
{
    public class ReviewRepo: AbstractClass<Review>,IReviewRepo
    {
        public ReviewRepo(BookBridgeDb context) : base(context)
        {
        }

        #region AddAsync

        public async Task<long> AddAsync(Review entity)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
            return await DbSet.MaxAsync(i => i.Id);
        }

        #endregion

        #region RemoveAsync

        public async Task<bool> RemoveAsync(long id)
        {
            var review = await DbSet.FindAsync(id)
                         ?? throw new KeyNotFoundException($"No review found by id: {id}");
            DbSet.Remove(review);
            await Context.SaveChangesAsync();
            return true;

        }
        #endregion

        #region UpdateAsync
        public async Task<bool> UpdateAsync(long id, Review entity)
        {
            var review = await DbSet.FindAsync(id)
                         ?? throw new KeyNotFoundException($"No review found by id: {id}");
            review.UpDateTime = entity.UpDateTime;
            review.BookId=entity.BookId;
            review.Comment=entity.Comment;
            review.Rating=entity.Rating;
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region SoftDeleteAsync

        public async Task<bool> SoftDeleteAsync(long id)
        {
            var review = await DbSet.FindAsync(id)
                         ?? throw new KeyNotFoundException($"No review found by id: {id}");
            review.IsActive = false;
            await Context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await DbSet.AsNoTracking()
                .Include(i=>i.Book)
                .Where(i => i.IsActive)
                .ToListAsync();
        }
        #endregion

        #region GetByIdAsync

        public async Task<Review> GetByIdAsync(long id)
        {
            var review = await DbSet.FindAsync(id)
                         ?? throw new KeyNotFoundException($"No review found by id: {id}");
            return review;
        }
        #endregion

        #region GetBookReviewsAsync

        public async Task<IEnumerable<Review>> GetBookReviewsAsync(long bookId)
        {
            var book = await Context.Books.FindAsync(bookId)
                       ?? throw new KeyNotFoundException($"No book found by id: {bookId}");
            var review = await DbSet.Where(i => i.BookId == bookId).AsNoTracking().ToListAsync();
            return review;
        }

        #endregion

        #region GetUserReviewsAsync

        public async Task<IEnumerable<Review>> GetUserReviewsAsync(string userId)
        {
            var user = await Context.Users.FindAsync(userId)
                       ?? throw new KeyNotFoundException($"No user found by id: {userId}");
            var review=await DbSet.Where(i=>i.UserId== userId).AsNoTracking().ToListAsync();
            return review;
        }
        #endregion
    }
}
