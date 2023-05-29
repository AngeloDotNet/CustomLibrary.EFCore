using System.Collections.Generic;
using System.Threading.Tasks;
using CustomLibrary.EFCore.EFCore.Core.Interfaces;
using NET6CustomLibrary.EFCore;

namespace CustomLibrary.EFCore.EFCore.Infrastructure.Interfaces;

public interface IDatabaseRepository<TEntity, TKey> : IDatabase<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    Task<ListViewModel<TEntity>> GetListPaginationAsync(int pageIndex, int pageSize);
    Task<List<TEntity>> GetOrderByIdAscendingAsync();
    Task<List<TEntity>> GetOrderByIdDescendingAsync();
    Task<int> GetCountAsync();
}