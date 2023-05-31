

namespace CustomLibrary.EFCore.EFCore.Infrastructure.Repository;

public class DatabaseRepository<TEntity, TKey> : Database<TEntity, TKey>, IDatabaseRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    public DatabaseRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<int> GetCountAsync()
    {
        var result = await DbContext.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync();

        var itemCount = result.Count;

        return itemCount;
    }

    public async Task<List<TEntity>> GetOrderByIdAscendingAsync()
    {
        return await DbContext.Set<TEntity>()
            .OrderBy(x => x.Id)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<TEntity>> GetOrderByIdDescendingAsync()
    {
        return await DbContext.Set<TEntity>()
            .OrderByDescending(x => x.Id)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ListViewModel<TEntity>> GetListPaginationAsync(int pageIndex, int pageSize)
    {
        var result = await DbContext.Set<TEntity>()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return new ListViewModel<TEntity> { Results = result, TotalCount = result.Count };
    }
}