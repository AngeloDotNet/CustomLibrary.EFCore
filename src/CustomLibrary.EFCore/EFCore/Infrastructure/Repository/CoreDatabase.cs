namespace CustomLibrary.EFCore.EFCore.Infrastructure.Repository;

public class CoreDatabase<TEntity, TKey> : Database<TEntity, TKey>, ICoreDatabase<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    public CoreDatabase(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<TEntity>> GetItemsAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
    Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();

        if (includes != null)
        {
            query = includes(query);
        }

        if (condition != null)
        {
            query = query.Where(condition);
        }

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<TEntity> GetItemByIdAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
        Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();

        if (includes != null)
        {
            query = includes(query);
        }

        if (condition != null)
        {
            query = query.Where(condition);
        }

        return await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetOrderedItemsAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
        Expression<Func<TEntity, bool>> conditionWhere, Expression<Func<TEntity, dynamic>> orderBy,
        OrderType orderType = OrderType.Ascending, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();

        if (includes != null)
        {
            query = includes(query);
        }

        if (conditionWhere != null)
        {
            query = query.Where(conditionWhere);
        }

        if (orderBy != null)
        {
            if (orderType == OrderType.Ascending)
            {
                query = query.OrderBy(orderBy);
            }

            if (orderType == OrderType.Descending)
            {
                query = query.OrderByDescending(orderBy);
            }
        }

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetItemsCountAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
        Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();

        if (includes != null)
        {
            query = includes(query);
        }

        if (condition != null)
        {
            query = query.Where(condition);
        }

        return await query.AsNoTracking().CountAsync(cancellationToken);
    }
}