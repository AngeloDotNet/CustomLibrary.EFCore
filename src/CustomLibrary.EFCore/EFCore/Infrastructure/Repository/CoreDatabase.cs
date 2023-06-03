namespace CustomLibrary.EFCore.EFCore.Infrastructure.Repository;

public class CoreDatabase<TEntity, TKey> : Database<TEntity, TKey>, ICoreDatabase<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    public CoreDatabase(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<int> GetCountAsync()
    {
        var result = await DbContext.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync();

        return result.Count;
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