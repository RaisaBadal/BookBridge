using AutoMapper;
using BookBridge.Application.Interfaces.StatisticInterfaces;
using BookBridge.Application.Models;
using BookBridge.Application.Models.Request;
using BookBridge.Application.Models.StatisticModels;
using BookBridge.Application.StaticFiles;
using BookBridge.Domain.Interfaces;

namespace BookBridge.Application.Services.StatisticServices
{
    public class StatisticBookRelatedService : AbstractService, IStatisticBookRelatedService
    {
        public StatisticBookRelatedService(IUnitOfWorkRepo unitOfWorkRepo, IMapper autoMapper) : base(unitOfWorkRepo, autoMapper)
        {
        }

        #region GetMostPopularBook
        public async Task<IEnumerable<PopularBooksModel>> GetMostPopularBook(DateModel dateModel)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(dateModel, nameof(dateModel));
                if (dateModel.StartDate > DateTime.Now || dateModel.EndDate > DateTime.Now
                    ||dateModel.StartDate>dateModel.EndDate) 
                    throw new ArgumentException(ErrorKeys.BadRequest);
                var borrowRecords = await UnitOfWorkRepo.BorrowRecord.ReturnAllBorrowRecordAsync();
                var filteredRecords= borrowRecords
                    .Where(i=>i.BorrowDate>=dateModel.StartDate
                    &&i.BorrowDate<=dateModel.EndDate)
                    .ToList();
                var groupedBooks = filteredRecords
                    .GroupBy(i => i.BookId)
                    .Select(i => new 
                    {
                        Id=i.First().BookId,
                        count=i.Count()
                    })
                .OrderByDescending(i=>i.count) 
                .ToList();
                            
                var popularBooks=new List<PopularBooksModel>();
                foreach (var item in groupedBooks)
                {
                    var book = await UnitOfWorkRepo.BookRepo.GetByIdAsync(item.Id);
                    if (book != null)
                    {
                        popularBooks.Add(new PopularBooksModel
                        {
                            Id=item.Id,
                            Title=book.Title,
                            count=item.count
                        });
                    }
                }
                return popularBooks;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetMostPopularAuthor

        public async Task<IEnumerable<AuthorModel>> GetMostPopularAuthor(DateModel dateModel)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(dateModel,nameof(dateModel));
                if (dateModel.StartDate >= DateTime.Now || dateModel.EndDate >= DateTime.Now
                    || dateModel.StartDate > dateModel.EndDate)
                    throw new ArgumentException(ErrorKeys.BadRequest);
                var borrowedRecord=await UnitOfWorkRepo.BorrowRecord.ReturnAllBorrowRecordAsync();
                var filterBorrowedRecord = borrowedRecord.Where(i => i.BorrowDate <= dateModel.StartDate
                && i.BorrowDate <= dateModel.EndDate);
                var groupedBooks = filterBorrowedRecord
                    .GroupBy(i => i.Books.Select(io=>io.AuthorId))
                    .Select(i => new
                    {
                        Id = i.First().Books.Select(io=>io.AuthorId).First(),
                        count = i.Count()
                    })
                    .OrderByDescending(i=>i.count);
                var authorsList=new List<AuthorModel>();
                foreach (var item in groupedBooks)
                {
                    var author=await UnitOfWorkRepo.AuthorRepo.GetByIdAsync(item.Id);
                    authorsList.Add(new AuthorModel
                    {
                        Id=author.Id,
                        Surname=author.Name,
                        Name=author.Name,
                        BirthDate=author.BirthDate,
                    });
                }
                return authorsList;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetMostPopularCatgory
        public async Task<IEnumerable<BookCategoryModel>> GetMostPopularCatgory(DateModel dateModel)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(dateModel, nameof(dateModel));
                if (dateModel.StartDate >= DateTime.Now || dateModel.EndDate >= DateTime.Now
                    || dateModel.StartDate > dateModel.EndDate)
                    throw new ArgumentException(ErrorKeys.BadRequest);
                var borrowedRecord = await UnitOfWorkRepo.BorrowRecord.ReturnAllBorrowRecordAsync();
                var filterBorrowedRecord = borrowedRecord.Where(i => i.BorrowDate <= dateModel.StartDate
                && i.BorrowDate <= dateModel.EndDate);
                var groupedBooks = filterBorrowedRecord
                .GroupBy(i => i.Books.Select(i=>i.BookCategoryId))
                .Select(i => new
                {
                    Id = i.First().Books.Select(i=>i.BookCategoryId).First(),
                    count = i.Count()
                })
                .OrderByDescending(i => i.count);
                var categoryList = new List<BookCategoryModel>();
                foreach (var item in groupedBooks)
                {
                    var category=await UnitOfWorkRepo.BookCategoryRepo.GetByIdAsync(item.Id);
                    categoryList.Add(new BookCategoryModel
                    {
                        Id=category.Id,
                        Description = category.Description,
                        Name = category.Name
                    });
                }

                return categoryList;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #endregion
    }
}
