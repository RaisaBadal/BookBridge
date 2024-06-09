namespace BookBridge.Application.Interfaces
{
    public interface ICrud<T,Tk> where T: class
    {
        Task<Tk> AddAsync(T entity);
        Task<bool> RemoveAsync(Tk entity);
        Task<bool> UpdateAsync(Tk id, T entity);
        Task<bool> SoftDeleteAsync(Tk id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Tk id);
    }
}
