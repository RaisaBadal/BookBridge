using BookBridge.Application.Interfaces.StatisticInterfaces;
using BookBridge.Application.Models.StatisticModels;
using BookBridge.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using BookBridge.Application.response;
using BookBridge.Application.StaticFiles;
using BookBridge.Application.Exceptions;
using BookBridge.Application.Models.Request;

namespace BookBridge.API.Controllers.StatisticController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticBookRelatedController : ControllerBase
    {
        private readonly IStatisticBookRelatedService statisticBookRelatedService;
        private readonly IMemoryCache memoryCache;

        public StatisticBookRelatedController(IStatisticBookRelatedService statisticBookRelatedService, 
            IMemoryCache memoryCache)
        {
                this.memoryCache = memoryCache;
                this.statisticBookRelatedService = statisticBookRelatedService;
        }

        [HttpPost]
        [Route(nameof(GetMostPopularBook))]
        public async Task<Response<IEnumerable<PopularBooksModel>>> GetMostPopularBook([FromBody]DateModel dateModel)
        {
            try
            {
                if (!ModelState.IsValid) throw new GeneralException(ErrorKeys.BadRequest);
                var res=await statisticBookRelatedService.GetMostPopularBook(dateModel);
                if(!res.Any()) throw new NotFoundException(ErrorKeys.NotFound);
                return Response<IEnumerable<PopularBooksModel>>.Ok(res);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<PopularBooksModel>>.Error(ex.Message, ex.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route(nameof (GetMostPopularAuthor))]
        public async Task<Response<IEnumerable<AuthorModel>>> GetMostPopularAuthor(DateModel dateModel)
        {
            try
            {
                if (!ModelState.IsValid) throw new GeneralException(ErrorKeys.BadRequest);
                var res = await statisticBookRelatedService.GetMostPopularAuthor(dateModel);
                if (!res.Any()) throw new NotFoundException(ErrorKeys.NotFound);
                return Response<IEnumerable<AuthorModel>>.Ok(res);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<AuthorModel>>.Error(ex.Message, ex.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route(nameof(GetMostPopularCatgory))]
        public async Task<Response<IEnumerable<BookCategoryModel>>> GetMostPopularCatgory(DateModel dateModel)
        {
            try
            {
                if (!ModelState.IsValid) throw new GeneralException(ErrorKeys.BadRequest);
                var res = await statisticBookRelatedService.GetMostPopularCatgory(dateModel);
                if (!res.Any()) throw new NotFoundException(ErrorKeys.NotFound);
                return Response<IEnumerable<BookCategoryModel>>.Ok(res);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<BookCategoryModel>>.Error(ex.Message, ex.StackTrace, ErrorKeys.InternalServerError);
            }
        }
    }
}
