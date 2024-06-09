using BookBridge.Application.Models.Request;

namespace BookBridge.Application.Interfaces
{
    public interface IBookCategoryService:ICrud<BookCategoryModel,long>
    {
    }
}
