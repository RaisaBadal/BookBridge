namespace BookBridge.Domain.Interfaces
{
    public interface ICrudRepo<T,TK> where T : class
    {
        Task<long> AddAsync(T entity);
        Task<bool> RemoveAsync(TK entity);
        Task<bool> UpdateAsync(TK id, T entity);
        Task<bool> SoftDeleteAsync(TK id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(TK id);

    }
}
