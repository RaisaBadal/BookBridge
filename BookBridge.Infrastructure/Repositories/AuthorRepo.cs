using BookBridge.Domain.Data;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Infrastructure.Repositories
{
    public class AuthorRepo:AbstractClass<Author>,IAuthorRepo
    {
        public AuthorRepo(BookBridgeDb context):base(context)
        {
            
        }

        #region AddAsync

        public async Task<long> AddAsync(Author entity)
        {
            var author = await Context.Authors.FirstOrDefaultAsync
            (i => i.Name == entity.Name
                  && i.Surname == entity.Surname
                  && i.BirthDate == entity.BirthDate);
            if (author != null) throw new ArgumentException("This author is exist in database");
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
            var maxId = await DbSet.MaxAsync(i => i.Id);
            return maxId;
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(long id)
        {
            var author = await DbSet.FindAsync(id);
            if (author is null) return false;
            DbSet.Remove(author);
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region UpdateAsync

        public async Task<bool> UpdateAsync(long id, Author entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            var authorId = await DbSet.FirstOrDefaultAsync(i => i.Id == id);
            if (authorId is null) return false; 
            authorId.BirthDate=entity.BirthDate;
            authorId.Name = entity.Name;
            authorId.Surname=entity.Surname;
            await Context.SaveChangesAsync();
            return true;    
        }

        #endregion

        #region SoftDeleteAsync

        public async Task<bool> SoftDeleteAsync(long id)
        {
            var author = await DbSet.FindAsync(id);
            if(author is null) return false;
            author.IsActive = false;
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await DbSet.AsNoTracking().Where(i => i.IsActive).ToListAsync();
        }

        #endregion

        #region GetByIdAsync

        public async Task<Author> GetByIdAsync(long id)
        {
            var author = await DbSet.FindAsync(id);
            return author ?? throw new ArgumentException("No author found with this id");
        }
        #endregion
    }
}
