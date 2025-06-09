namespace BusinessLayer
{
    public interface IDbService <T,K>
    {
        Task CreateAsync(T obj);
        Task<List<T>> ReadAllAsync();
        Task<T> ReadAsync(K id);
        Task UpdateAsync(T obj);
        Task DeleteAsync(K id);
        Task ClearAsync();
    }
}
