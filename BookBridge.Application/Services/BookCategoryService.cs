using AutoMapper;
using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.StaticFiles;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;

namespace BookBridge.Application.Services
{
    public class BookCategoryService(IUnitOfWorkRepo unitOfWorkRepo, IMapper autoMapper) :AbstractService(unitOfWorkRepo, autoMapper),IBookCategoryService
    {

        #region AddAsync

        public async Task<long> AddAsync(BookCategoryModel entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity, ErrorKeys.ArgumentNull);
                if (entity is not { Description: null, Name: null })
                {
                    var mapped = autoMapper.Map<BookCategory>(entity)
                                 ?? throw new ArgumentException(ErrorKeys.Mapped);
                    var category = await unitOfWorkRepo.BookCategoryRepo.AddAsync(mapped);
                    return category;

                }
                else
                {
                    throw new ArgumentNullException(ErrorKeys.ArgumentNull);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region RemoveAsync

        public async Task<bool> RemoveAsync(long id)
        {
            try
            {
                if (id < -1) throw new ArgumentException(ErrorKeys.BadRequest);
                var res=await unitOfWorkRepo.BookCategoryRepo.RemoveAsync(id);
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region UpdateAsync

        public async Task<bool> UpdateAsync(long id, BookCategoryModel entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity);
                if (entity is not { Description: null, Name: null })
                {
                    var mapped = AutoMapper.Map<BookCategory>(entity)
                                 ?? throw new ArgumentException(ErrorKeys.Mapped);
                    var category = await unitOfWorkRepo.BookCategoryRepo.UpdateAsync(id, mapped);
                    return category;
                }
                else
                {
                    throw new ArgumentNullException(ErrorKeys.ArgumentNull);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        #endregion

        #region SoftDeleteAsync

        public async Task<bool> SoftDeleteAsync(long id)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                var category = await unitOfWorkRepo.BookCategoryRepo.SoftDeleteAsync(id);
                return category;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<BookCategoryModel>> GetAllAsync()
        {
            try
            {
                var category = await UnitOfWorkRepo.BookCategoryRepo.GetAllAsync();
                var mappedCategoryModel = autoMapper.Map<IEnumerable<BookCategoryModel>>(category);
                return mappedCategoryModel;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region GetByIdAsync

        public async Task<BookCategoryModel> GetByIdAsync(long id)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                var book = await unitOfWorkRepo.BookCategoryRepo.GetByIdAsync(id);
                ArgumentNullException.ThrowIfNull(book);
                var mapped = autoMapper.Map<BookCategoryModel>(book)
                             ?? throw new ArgumentNullException(ErrorKeys.ArgumentNull);
                return mapped;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion
    }
}
