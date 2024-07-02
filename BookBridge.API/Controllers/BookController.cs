using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.response;
using BookBridge.Application.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BookBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="ADMIN")]
    public class BookController(
        IBookService _bookService,
        IBookCategoryService _bookCategoryService,
        IAuthorService _authorService,
        IMemoryCache _memoryCache)
        : ControllerBase
    {
        private readonly IMemoryCache _memoryCache=_memoryCache;


        //BookEndpoints
        [HttpPost]
        [Route(nameof(InsertBook))]
        [AllowAnonymous]
        public async Task<Response<long>> InsertBook([FromBody] BookModel entity)
        {

            if (!ModelState.IsValid || entity is null) return Response<long>.Error(ErrorKeys.BadRequest);
            var res = await _bookService.AddAsync(entity);
            return res != -1 ? Response<long>.Ok(res) : Response<long>.Error(ErrorKeys.BadRequest);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        [AllowAnonymous]
        public async Task<Response<bool>> RemoveBook([FromRoute] long id)
        {

            var res = await _bookService.RemoveAsync(id);
            return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.BadRequest);

        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<ActionResult<bool>> UpdateBook([FromRoute] long id, [FromBody] BookModel entity)
        {

            if (!ModelState.IsValid || entity is null) return BadRequest(ErrorKeys.BadRequest);
            var res = await _bookService.UpdateAsync(id, entity);
            return res ? Ok(res) : BadRequest(ErrorKeys.BadRequest);

        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> SoftDeleteBook([FromRoute] long id)
        {

            var res = await _bookService.SoftDeleteAsync(id);
            return Response<bool>.Ok(res);

        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<BookModel>> AllBook()
        {

            const string cacheKey = "GetAllBook";
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<BookModel>? cachedData))
            {
                if (cachedData != null) return Ok(cachedData);
            }
            else
            {
                var res = await _bookService.GetAllAsync();
                if (!res.Any())
                {
                    BadRequest(res);
                }

                _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(30));
                return Ok(res);
            }
            return BadRequest(ErrorKeys.BadRequest);


        }

        [HttpGet]
        [Route("[action]/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BookModel>> GetByIdBook([FromRoute] long id)
        {

            var cacheKey = $"BookById {id}";
            if (_memoryCache.TryGetValue(cacheKey, out BookModel? model))
            {
                if (model != null) return Ok(model);
            }
            var res = await _bookService.GetByIdAsync(id);
            if (res is null) return BadRequest(ErrorKeys.BadRequest);
            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(30));
            return Ok(res);


        }

        //BookCategoryEndpoint
        [HttpPost]
        [Route(nameof(InsertBookCategory))]
        public async Task<Response<long>> InsertBookCategory([FromBody] BookCategoryModel entity)
        {

            if (!ModelState.IsValid || entity is null) return Response<long>.Error(ErrorKeys.BadRequest);
            var res = await _bookCategoryService.AddAsync(entity);
            return res != -1 ? Response<long>.Ok(res) : Response<long>.Error(ErrorKeys.BadRequest);

        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> RemoveBookCategory([FromRoute] long id)
        {

            var res = await _bookCategoryService.RemoveAsync(id);
            return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.BadRequest);

        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> UpdateBookCategory([FromRoute] long id, [FromBody] BookCategoryModel entity)
        {

            if (!ModelState.IsValid || entity is null) return Response<bool>.Error(ErrorKeys.BadRequest);
            var res = await _bookCategoryService.UpdateAsync(id, entity);
            return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.BadRequest);
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> SoftDeleteBookCategory([FromRoute] long id)
        {

            var res = await _bookCategoryService.SoftDeleteAsync(id);
            return Response<bool>.Ok(res);

        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookCategoryModel>>> AllBookCategory()
        {

            const string cacheKey = "GetAllBookCategory";
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<BookCategoryModel>? cachedData))
            {
                if (cachedData != null) return Ok(cachedData);
            }
            else
            {
                var res = await _bookCategoryService.GetAllAsync();
                if (!res.Any())
                {
                    return BadRequest(ErrorKeys.BadRequest);
                }

                _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(30));
                return Ok(res);
            }
            return BadRequest(ErrorKeys.BadRequest);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BookCategoryModel>> GetByIdBookCategory([FromRoute] long id)
        {

            var cacheKey = $"BookCategoryById {id}";
            if (_memoryCache.TryGetValue(cacheKey, out BookCategoryModel? model))
            {
                if (model != null) return Ok(model);
            }
            var res = await _bookCategoryService.GetByIdAsync(id);
            if (res is null) return BadRequest(ErrorKeys.BadRequest);
            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(30));
            return Ok(res);

        }

        //AuthorEndpoint
        [HttpPost]
        [Route(nameof(InsertAuthor))]
        public async Task<Response<long>> InsertAuthor([FromBody] AuthorModel entity)
        {

            if (!ModelState.IsValid || entity is null) return Response<long>.Error(ErrorKeys.BadRequest);
            var res = await _authorService.AddAsync(entity);
            return res != -1 ? Response<long>.Ok(res) : Response<long>.Error(ErrorKeys.BadRequest);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> RemoveAuthor([FromRoute] long id)
        {

            var res = await _authorService.RemoveAsync(id);
            return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.BadRequest);

        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> UpdateAuthor([FromRoute] long id, [FromBody] AuthorModel entity)
        {

            if (!ModelState.IsValid || entity is null) return Response<bool>.Error(ErrorKeys.BadRequest);
            var res = await _authorService.UpdateAsync(id, entity);
            return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.BadRequest);

        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> SoftDeleteAuthor([FromRoute] long id)
        {

            var res = await _authorService.SoftDeleteAsync(id);
            return Response<bool>.Ok(res);

        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AuthorModel>>> AllAuthor()
        {

            const string cacheKey = "GetAllAuthors";
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<AuthorModel>? cachedData))
            {
                if (cachedData != null) return Ok(cachedData);
            }
            else
            {
                var res = await _authorService.GetAllAsync();
                if (!res.Any())
                {
                    return BadRequest(ErrorKeys.BadRequest);
                }

                _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(30));
                return Ok(res);
            }
            return BadRequest(ErrorKeys.BadRequest);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthorModel>> GetByIdAuthor([FromRoute] long id)
        {

            var cacheKey = $"AuthorById {id}";
            if (_memoryCache.TryGetValue(cacheKey, out AuthorModel? model))
            {
                if (model != null) return Ok(model);
            }
            var res = await _authorService.GetByIdAsync(id);
            if (res is null) return BadRequest(ErrorKeys.BadRequest);
            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(30));
            return Ok(res);
        }
    }
}
