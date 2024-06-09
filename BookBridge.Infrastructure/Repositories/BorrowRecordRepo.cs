using BookBridge.Domain.Data;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Infrastructure.Repositories
{
    public class BorrowRecordRepo:AbstractClass<BorrowRecord>,IBorrowRecord
    {
        public BorrowRecordRepo(BookBridgeDb context) : base(context)
        {
        }

        #region BorrowBookAsync

        public async Task<BorrowRecord> BorrowBookAsync(long bookId, string userId)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync();

            var book = await Context.Books.FindAsync(bookId);
            var user = await Context.Users.FindAsync(userId);
            var us = await Context.BorrowRecords.OrderBy(io=>io.Id).LastOrDefaultAsync(i=>i.UserId==userId);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book with id: {bookId} is not found.");
            }

            if (user == null)
            {
                throw new KeyNotFoundException($"User with id: {userId} is not found.");
            }

            if (book.AvailableCopies == 0)
            {
                throw new InvalidOperationException("No available copies of the book.");
            }
            
            if (us is not null && !us.IsReturned)
            {
                throw new InvalidOperationException("You cannot take a new book, please return the last book first.");
            }

            book.AvailableCopies -= 1;

            var borrowRecord = new BorrowRecord
            {
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(5),
                BookId = bookId,
                UserId = userId,
                IsReturned = false
            };

            await Context.BorrowRecords.AddAsync(borrowRecord);

            await Context.SaveChangesAsync();
            await transaction.CommitAsync();

            return borrowRecord;

        }
        #endregion

        #region ReturnBookAsync
        public async Task<BorrowRecord> ReturnBookAsync(long bookId, string userId)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync();
            var book = await Context.Books.FindAsync(bookId);
            var user = await Context.Users.FindAsync(userId);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book with id: {bookId} is not found.");
            }

            if (user == null)
            {
                throw new KeyNotFoundException($"User with id: {userId} is not found.");
            }

            var borrowRecord = await DbSet.FirstOrDefaultAsync
            (i => i.UserId == userId
                  && i.BookId == bookId && !i.IsReturned) ?? throw new InvalidOperationException("No borrow record is found by your info");
            borrowRecord.IsReturned=true;
            borrowRecord.ReturnDate = DateTime.Now;
            book.AvailableCopies += 1;
            await Context.SaveChangesAsync();
            await transaction.CommitAsync();
            return borrowRecord;

        }
        #endregion
    }
}
