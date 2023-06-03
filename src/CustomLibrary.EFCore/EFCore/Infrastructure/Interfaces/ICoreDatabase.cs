namespace CustomLibrary.EFCore.EFCore.Infrastructure.Interfaces;

public interface ICoreDatabase<TEntity, TKey> : IDatabase<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    Task<ListViewModel<TEntity>> GetListPaginationAsync(int pageIndex, int pageSize);
    Task<List<TEntity>> GetOrderByIdAscendingAsync();
    Task<List<TEntity>> GetOrderByIdDescendingAsync();
    Task<int> GetCountAsync();
}