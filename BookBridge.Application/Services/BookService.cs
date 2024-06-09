using AutoMapper;
using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.StaticFiles;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;

namespace BookBridge.Application.Services
{
    public class BookService(IUnitOfWorkRepo unitOfWorkRepo, IMapper autoMapper) :AbstractService(unitOfWorkRepo, autoMapper), IBookService
    {

        #region AddAsync

        public async Task<long> AddAsync(BookModel entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity, ErrorKeys.ArgumentNull);
                if (entity is not { Title: null, Description: null })
                {
                    var mapped = autoMapper.Map<Book>(entity)
                                 ?? throw new ArgumentException(ErrorKeys.Mapped);
                    var book = await unitOfWorkRepo.BookRepo.AddAsync(mapped);
                    return book;

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
                var res = await unitOfWorkRepo.BookRepo.RemoveAsync(id);
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
        public async Task<bool> UpdateAsync(long id, BookModel entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity);
                if (entity is not { Title: null, Description: null })
                {
                    var mapped = AutoMapper.Map<Book>(entity)
                                 ?? throw new ArgumentException(ErrorKeys.Mapped);
                    var book = await unitOfWorkRepo.BookRepo.UpdateAsync(id, mapped);
                    return book;
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
                var book = await unitOfWorkRepo.BookRepo.SoftDeleteAsync(id);
                return book;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<BookModel>> GetAllAsync()
        {
            try
            {
                var book = await UnitOfWorkRepo.BookRepo.GetAllAsync();
                var mappedAuthorModel = autoMapper.Map<IEnumerable<BookModel>>(book);
                return mappedAuthorModel;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region GetByIdAsync

        public async Task<BookModel> GetByIdAsync(long id)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                var book = await unitOfWorkRepo.BookRepo.GetByIdAsync(id);
                ArgumentNullException.ThrowIfNull(book);
                var mapped = autoMapper.Map<BookModel>(book)
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
