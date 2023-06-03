namespace CustomLibrary.EFCore.EFCore.Infrastructure.Repository;

public class UnitOfWork<TEntity, TKey> : IUnitOfWork<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    public DbContext DbContext { get; }
    public ICoreDatabase<TEntity, TKey> ReadOnly { get; }
    public ICoreCommand<TEntity, TKey> Command { get; }

    public UnitOfWork(DbContext dbContext, ICoreDatabase<TEntity, TKey> coreDatabase, ICoreCommand<TEntity, TKey> coreCommand)
    {
        DbContext = dbContext;
        ReadOnly = coreDatabase;
        Command = coreCommand;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DbContext.Dispose();
        }
    }
}