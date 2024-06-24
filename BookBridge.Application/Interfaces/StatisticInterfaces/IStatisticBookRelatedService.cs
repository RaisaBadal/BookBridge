using BookBridge.Application.Models;
using BookBridge.Application.Models.Request;
using BookBridge.Application.Models.StatisticModels;

namespace BookBridge.Application.Interfaces.StatisticInterfaces
{
    public interface IStatisticBookRelatedService
    {
        Task<IEnumerable<PopularBooksModel>> GetMostPopularBook(DateModel dateModel);

        Task<IEnumerable<AuthorModel>> GetMostPopularAuthor(DateModel dateModel);

        Task<IEnumerable<BookCategoryModel>> GetMostPopularCatgory(DateModel dateModel);
    }
}
