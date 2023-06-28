# DBContext generic methods

## Example entity
```csharp
public class MyEntity : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

## Example interface
```csharp
public interface IMyService
{
    Task<List<MyEntity>> GetListItemAsync();
    Task<MyEntity> GetItemAsync(int id);
    Task CreateItemAsync(MyEntity item);
    Task UpdateItemAsync(MyEntity item);
    Task DeleteItemAsync(MyEntity item);
}
```

## Example class
```csharp
public class MyService : IMyService
{
    private readonly IUnitOfWork<MyEntity, int> unitOfWork;

    public MyService(IUnitOfWork<MyEntity, int> unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<List<MyEntity>> GetListItemAsync()
    {
        var listItem = await unitOfWork.ReadOnly.GetAllAsync();
        return listItem;
    }

    public async Task<MyEntity> GetItemAsync(int id)
    {
        var item = await unitOfWork.ReadOnly.GetByIdAsync(id);
        return item;
    }

    public async Task CreateItemAsync(MyEntity item)
    {
        await unitOfWork.Command.CreateAsync(item);
    }

    public async Task UpdateItemAsync(MyEntity item)
    {
        await unitOfWork.Command.UpdateAsync(item);
    }

    public async Task DeleteItemAsync(MyEntity item)
    {
        await unitOfWork.Command.DeleteAsync(item);
    }
}
```
