# Custom Library Entity Framework Core
Collection of tools mostly used in my private and/or work projects thus avoiding the duplication of repetitive code.

[![NuGet](https://img.shields.io/nuget/v/CustomLibrary.EFCore.svg?style=for-the-badge)](https://www.nuget.org/packages/CustomLibrary.EFCore)
[![NuGet](https://img.shields.io/nuget/dt/CustomLibrary.EFCore.svg?style=for-the-badge)](https://www.nuget.org/packages/CustomLibrary.EFCore)
[![GitHub License](https://img.shields.io/github/license/AngeloDotNet/CustomLibrary.EFCore?style=for-the-badge)](https://github.com/AngeloDotNet/CustomLibrary.EFCore/blob/main/LICENSE)

## :star: Give a star
If you found this Implementation helpful or used it in your Projects, do give it a :star: on Github. Thanks!

## :dvd: Installation
The library is available on [NuGet](https://www.nuget.org/packages/CustomLibrary.EFCore) or run the following command in the .NET CLI:

```bash
dotnet add package CustomLibrary.EFCore
```

## :memo: Library documentation
The extensions methods available regarding:

- [x] DBContext generic methods<br>
- [x] DBContext Pool registration for different databases (MySQL / MariaDB, PostgreSQL, SQLite, SQL server)
- [ ] Health checks with UI for different databases (MySQL / MariaDB, PostgreSQL, SQL server)

## Registering services at Startup
```csharp
public void ConfigureServices(IServiceCollection services)
{
  //numbers of retries to connect to the database
  int retryOnFailure = 3; // or zero if you don't want to retry

  //default assembly migrations (in this case the migrations will be created in the DbContext assembly)
  string migrationsAssembly = string.Empty;

  //or you can customize the assembly for migrations using the syntax
  string migrationsAssembly = "MyAssembly.Migrations.MySQLDatabase";

  services.AddDbContextGenericsMethods<MyDbContext>();

  //if you use MySQL database
  services.AddDbContextUseMySql(connectionString, retryOnFailure, migrationsAssembly);

  //if you use PostgreSQL database
  services.AddDbContextUsePostgres(connectionString, retryOnFailure, migrationsAssembly);

  //if you use SQLServer database
  services.AddDbContextUseSQLServer(connectionString, retryOnFailure, migrationsAssembly);

  //if you use SQLite database
  //in this case the retryOnFailure is not necessary as SQLite is not subject to transient errors
  services.AddDbContextUseSQLite(connectionString, migrationsAssembly);
}
```

## Connection strings of the databases
```json
// for database MySQL / MariaDB
"ConnectionStrings": {
  "Default": "Server=[SERVER];Database=[DATABASE];Uid=[USERNAME];Pwd=[PASSWORD];Port=3306"
}

//for database PostgreSQL
"ConnectionStrings": {
  "Default": "Host=[SERVER];Port=5432;Database=[DATABASE];Username=[USERNAME];Password=[PASSWORD]"
}

//for database SQLServer
"ConnectionStrings": {
  "Default": "Data Source=[SERVER];Initial Catalog=[DATABASE];User ID=[USERNAME];Password=[PASSWORD]"
//or "Default": "Data Source=[SERVER];Initial Catalog=[DATABASE];User ID=[USERNAME];Password=[PASSWORD];Encrypt=False"
}

//for database SQLite
"ConnectionStrings": {
    "Default": "Data Source=Data/MyDatabase.db"
}
```

## Example entity interface
```csharp
public class MyEntity : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
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

## :muscle: Contributing

Contributions and/or suggestions are always welcome.
