using BookBridge.Domain.Data;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Infrastructure.Repositories
{
    public class BookRepo:AbstractClass<Book>,IBookRepo
    {
        public BookRepo(BookBridgeDb context) : base(context)
        {
        }

        #region AddAsync
        public async Task<long> AddAsync(Book entity)
        {
            try
            {

                var book = await DbSet.FirstOrDefaultAsync(i => i.Title == entity.Title && i.Description == entity.Description);
                if (book is not null)
                {
                    book.TotalCopies += entity.TotalCopies;
                    book.AvailableCopies += entity.TotalCopies;
                    await Context.SaveChangesAsync();
                    return book.Id;
                }

                if (!await Context.Authors.AnyAsync(io => io.Id == entity.AuthorId))
                {
                    throw new ArgumentException("Author don't found");
                }

                if (!await Context.BookCategories.AnyAsync(io => io.Id == entity.BookCategoryId))
                {
                    throw new ArgumentException("BookCategory don't found");
                }

                entity.AvailableCopies= entity.TotalCopies;
                await DbSet.AddAsync(entity);

                await Context.SaveChangesAsync();
                return await DbSet.MaxAsync(i => i.Id);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region RemoveAsync

        public async Task<bool> RemoveAsync(long id)
        {
            var book = await DbSet.FindAsync(id);
            if (book is null) return false;
            DbSet.Remove(book);
            await Context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region UpdateAsync

        public async Task<bool> UpdateAsync(long id, Book entity)
        {
            var book = await DbSet.FindAsync(id);
            if (book is null) return false;
            book.AvailableCopies = entity.AvailableCopies;
            book.TotalCopies=entity.TotalCopies;
            book.AuthorId=entity.AuthorId;
            book.BookCategoryId=entity.BookCategoryId;
            book.CoverImageUrl = entity.CoverImageUrl;
            book.Description=entity.Description;
            book.Title=entity.Title;
            book.PublishedDate=entity.PublishedDate;
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region SoftDeleteAsync

        public async Task<bool> SoftDeleteAsync(long id)
        {
            var book = await DbSet.FirstOrDefaultAsync(i => i.Id == id);
            if (book is null) return false;
            book.IsActive = false;
            await Context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await DbSet.AsNoTracking()
                .Include(i => i.Authors)
                .Where(i => i.IsActive)
                .ToListAsync();
        }
        #endregion

        #region GetByIdAsync
        public async Task<Book> GetByIdAsync(long id)
        {
            var book = await DbSet.FindAsync(id);
            if (book != null)
            {
                return book;
            }
            throw new KeyNotFoundException($"No book found with the id: {id}");

        }


        #endregion
    }
}
