# DBContext Pool registration for different databases

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

  services.AddDbContextServicesGenerics<MyDbContext>();

  //if you use MySQL database
  services.AddDbContextForMySql(connectionString, retryOnFailure, migrationsAssembly);

  //if you use PostgreSQL database
  services.AddDbContextForPostgres(connectionString, retryOnFailure, migrationsAssembly);

  //if you use SQLServer database
  services.AddDbContextForSQLServer(connectionString, retryOnFailure, migrationsAssembly);

  //if you use SQLite database but in this case the retryOnFailure is not necessary as SQLite is not subject to transient errors
  services.AddDbContextForSQLite(connectionString, migrationsAssembly);
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
