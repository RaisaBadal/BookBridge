using AutoMapper;
using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.StaticFiles;
using BookBridge.Domain.Entities;
using BookBridge.Domain.Interfaces;
using Exception = System.Exception;

namespace BookBridge.Application.Services
{
    public class AuthorService(IUnitOfWorkRepo unitOfWorkRepo, IMapper autoMapper) : AbstractService(unitOfWorkRepo, autoMapper), IAuthorService
    {

        #region AddAsync

        public async Task<long> AddAsync(AuthorModel entity)
        {
            try
            { 
                ArgumentNullException.ThrowIfNull(entity, ErrorKeys.ArgumentNull);
                if (entity is not { Surname: null, Name: null})
                {
                   var mapped=autoMapper.Map<Author>(entity) 
                              ?? throw new ArgumentException(ErrorKeys.Mapped);
                   var author= await unitOfWorkRepo.AuthorRepo.AddAsync(mapped);
                   return author;

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
                var author=await unitOfWorkRepo.AuthorRepo.RemoveAsync(id); 
                return author;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        #endregion

        #region UpdateAsync

        public async Task<bool> UpdateAsync(long id, AuthorModel entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity);
                if (entity is not { Surname: null, Name: null })
                {
                    var mapped = AutoMapper.Map<Author>(entity)
                                 ?? throw new ArgumentException(ErrorKeys.Mapped);
                    var author = await unitOfWorkRepo.AuthorRepo.UpdateAsync(id, mapped);
                    return author;
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
                var author = await unitOfWorkRepo.AuthorRepo.SoftDeleteAsync(id);
                return author;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<AuthorModel>> GetAllAsync()
        {
            try
            {
                var author = await UnitOfWorkRepo.AuthorRepo.GetAllAsync();
                var mappedAuthorModel = autoMapper.Map<IEnumerable<AuthorModel>>(author);
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

        public async Task<AuthorModel> GetByIdAsync(long id)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                var author=await unitOfWorkRepo.AuthorRepo.GetByIdAsync(id);
                ArgumentNullException.ThrowIfNull(author);
                var mapped = autoMapper.Map<AuthorModel>(author)
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
