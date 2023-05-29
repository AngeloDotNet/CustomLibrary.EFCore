using System;
using CustomLibrary.EFCore.EFCore.Core.Interfaces;

namespace CustomLibrary.EFCore.EFCore.Infrastructure.Interfaces;

public interface IUnitOfWork<TEntity, TKey> : IDisposable where TEntity : class, IEntity<TKey>, new()
{
    IDatabaseRepository<TEntity, TKey> ReadOnly { get; }
    ICommandRepository<TEntity, TKey> Command { get; }
}