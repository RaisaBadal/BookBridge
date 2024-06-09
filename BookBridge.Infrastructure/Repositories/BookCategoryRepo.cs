using BookBridge.Domain.Data;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Infrastructure.Repositories
{
    public class BookCategoryRepo : AbstractClass<BookCategory>, IBookCategoryRepo
    {
        public BookCategoryRepo(BookBridgeDb context):base(context)
        {
            
        }

        #region AddAsync

        public async Task<long> AddAsync(BookCategory entity)
        {
            var category = await DbSet.FirstOrDefaultAsync(i => i.Name == entity.Name);
            if (category is not null) throw new ArgumentException("this category is already exists");
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
            return await DbSet.MaxAsync(i => i.Id);
        }
        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<BookCategory>> GetAllAsync()
        {
            return await DbSet.AsNoTracking().Where(i => i.IsActive).ToListAsync();
        }
        #endregion

        #region GetByIdAsync
        public async Task<BookCategory> GetByIdAsync(long id)
        {
            var category = await DbSet.FirstOrDefaultAsync(i => i.Id == id);
            return category ?? throw new ArgumentException($"No category is found by id: {id}");
        }
        #endregion

        #region RemoveAsync

        public async Task<bool> RemoveAsync(long id)
        {
            var category = await DbSet.FindAsync(id);
            if (category is null) return false;
            DbSet.Remove(category);
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region SoftDeleteAsync

        public async Task<bool> SoftDeleteAsync(long id)
        {
            var category = await DbSet.FindAsync(id);
            if (category is null) return false;
            category.IsActive = false;
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region UpdateAsync
        public async Task<bool> UpdateAsync(long id, BookCategory entity)
        {
            var category = await DbSet.FindAsync(id);
            if (category is null) return false;
            category.Name = entity.Name;
            category.Description=entity.Description;
            await Context.SaveChangesAsync();
            return true;
        }

        #endregion
    }
}
