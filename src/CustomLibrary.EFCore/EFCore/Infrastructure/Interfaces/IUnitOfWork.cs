namespace CustomLibrary.EFCore.EFCore.Infrastructure.Interfaces;

public interface IUnitOfWork<TEntity, TKey> : IDisposable where TEntity : class, IEntity<TKey>, new()
{
    ICoreDatabase<TEntity, TKey> ReadOnly { get; }
    ICoreCommand<TEntity, TKey> Command { get; }
}