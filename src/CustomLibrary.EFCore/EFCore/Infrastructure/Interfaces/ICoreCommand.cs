namespace CustomLibrary.EFCore.EFCore.Infrastructure.Interfaces;

public interface ICoreCommand<TEntity, TKey> : ICommand<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
}