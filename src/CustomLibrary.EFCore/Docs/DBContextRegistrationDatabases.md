# DBContext registration for databases SQL server

## Registering services at Startup
```csharp
public void ConfigureServices(IServiceCollection services)
{
  //default assembly migrations (in this case the migrations will be created in the DbContext assembly)
  string migrationsAssembly = string.Empty;

  //or you can customize the assembly for migrations using the syntax
  string migrationsAssembly = "MyAssembly.Migrations.MySQLDatabase";

  services.AddDbContextServicesGenerics<MyDbContext>();
  services.AddDbContextNoPoolSQLServer<MyDbContext>(connectionString, migrationsAssembly);
}
```

## Connection strings of the databases
```json
//for database SQLServer
"ConnectionStrings": {
  "Default": "Data Source=[SERVER];Initial Catalog=[DATABASE];User ID=[USERNAME];Password=[PASSWORD]"
  //or "Default": "Data Source=[SERVER];Initial Catalog=[DATABASE];User ID=[USERNAME];Password=[PASSWORD];Encrypt=False"
}
```
