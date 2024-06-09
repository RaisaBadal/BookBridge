using BookBridge.Domain.Entities;

namespace BookBridge.Domain.Interfaces
{
    public interface IBookRepo:ICrudRepo<Book,long>
    {
    }
}
