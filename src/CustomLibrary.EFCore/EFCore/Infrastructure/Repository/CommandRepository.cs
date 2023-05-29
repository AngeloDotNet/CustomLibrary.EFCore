using CustomLibrary.EFCore.EFCore.Core;
using CustomLibrary.EFCore.EFCore.Core.Interfaces;
using CustomLibrary.EFCore.EFCore.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomLibrary.EFCore.EFCore.Infrastructure.Repository;

public class CommandRepository<TEntity, TKey> : Command<TEntity, TKey>, ICommandRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
{
    public CommandRepository(DbContext dbContext) : base(dbContext)
    {
    }
}