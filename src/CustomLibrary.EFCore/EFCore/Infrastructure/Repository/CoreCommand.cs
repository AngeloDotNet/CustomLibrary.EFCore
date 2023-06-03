namespace CustomLibrary.EFCore.EFCore.Infrastructure.Repository;

public class CoreCommand<TEntity, TKey> : Command<TEntity, TKey>, ICoreCommand<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    public CoreCommand(DbContext dbContext) : base(dbContext)
    {
    }
}