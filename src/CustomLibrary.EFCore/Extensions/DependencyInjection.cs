namespace CustomLibrary.EFCore.Extensions;

public static class DependencyInjection
{
    #region "DBContext and relative extensions methods"

    /// <summary>
    /// Extension method to add register DbContext services (DbContext and UnitOfWork implementations)
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <returns>Registration of services</returns>
    public static IServiceCollection AddDbContextServicesGenerics<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
    {
        services.AddScoped<DbContext, TDbContext>();
        services.AddScoped(typeof(IUnitOfWork<,>), typeof(UnitOfWork<,>));
        services.AddScoped(typeof(ICoreDatabase<,>), typeof(CoreDatabase<,>));
        services.AddScoped(typeof(ICoreCommand<,>), typeof(CoreCommand<,>));

        return services;
    }

    /// <summary>
    /// Extension method to add a DbContext of type TDbContext with MySQL / MariaDB provider
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <param name="retryOnFailure"></param>
    /// <param name="migrationsAssembly"></param>
    /// <returns></returns>
    public static IServiceCollection AddDbContextForMySql<TDbContext>(this IServiceCollection services, string connectionString, int retryOnFailure, string migrationsAssembly) where TDbContext : DbContext
    {
        services.AddDbContextPool<TDbContext>(optionBuilder =>
        {
            if (retryOnFailure > 0)
            {
                optionBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
                {
                    // Abilito il connection resiliency (Provider di Mysql / MariaDB è soggetto a errori transienti)
                    // Info su: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                    options.EnableRetryOnFailure(retryOnFailure);
                    options.MigrationsAssembly(GeneratePathMigrations<TDbContext>(migrationsAssembly: migrationsAssembly));
                });
            }
            else
            {
                optionBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
                {
                    options.MigrationsAssembly(GeneratePathMigrations<TDbContext>(migrationsAssembly: migrationsAssembly));
                });
            }
        });
        return services;
    }

    /// <summary>
    /// Extension method to add a DbContext of type TDbContext with PostgreSQL provider
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <param name="retryOnFailure"></param>
    /// <param name="migrationsAssembly"></param>
    /// <returns></returns>
    public static IServiceCollection AddDbContextForPostgres<TDbContext>(this IServiceCollection services, string connectionString, int retryOnFailure, string migrationsAssembly) where TDbContext : DbContext
    {
        services.AddDbContextPool<TDbContext>(optionBuilder =>
        {
            if (retryOnFailure > 0)
            {
                optionBuilder.UseNpgsql(connectionString, options =>
                {
                    // Abilito il connection resiliency (Provider di Postgres è soggetto a errori transienti)
                    // Info su: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                    options.EnableRetryOnFailure(retryOnFailure);
                    options.MigrationsAssembly(GeneratePathMigrations<TDbContext>(migrationsAssembly: migrationsAssembly));
                });
            }
            else
            {
                optionBuilder.UseNpgsql(connectionString, options =>
                {
                    options.MigrationsAssembly(GeneratePathMigrations<TDbContext>(migrationsAssembly: migrationsAssembly));
                });
            }
        });

        return services;
    }

    /// <summary>
    /// Extension method to add a DbContext of type TDbContext with SQL Server provider
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <param name="retryOnFailure"></param>
    /// <param name="migrationsAssembly"></param>
    /// <returns></returns>
    public static IServiceCollection AddDbContextForSQLServer<TDbContext>(this IServiceCollection services, string connectionString, int retryOnFailure, string migrationsAssembly) where TDbContext : DbContext
    {
        services.AddDbContextPool<TDbContext>(optionBuilder =>
        {
            if (retryOnFailure > 0)
            {
                optionBuilder.UseSqlServer(connectionString, options =>
                {
                    // Abilito il connection resiliency (Provider di SQL Server è soggetto a errori transienti)
                    // Info su: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                    options.EnableRetryOnFailure(retryOnFailure);
                    options.MigrationsAssembly(GeneratePathMigrations<TDbContext>(migrationsAssembly: migrationsAssembly));
                });
            }
            else
            {
                optionBuilder.UseSqlServer(connectionString, options =>
                {
                    options.MigrationsAssembly(GeneratePathMigrations<TDbContext>(migrationsAssembly: migrationsAssembly));
                });
            }
        });

        return services;
    }

    /// <summary>
    /// Extension method to add a DbContext of type TDbContext with SQLite provider
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <param name="migrationsAssembly"></param>
    /// <returns></returns>
    public static IServiceCollection AddDbContextForSQLite<TDbContext>(this IServiceCollection services, string connectionString, string migrationsAssembly) where TDbContext : DbContext
    {
        services.AddDbContextPool<TDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlite(connectionString, options =>
            {
                // Non abilito il connection resiliency, non è supportato dal provider di Sqlite in quanto non soggetto a errori transienti)
                options.MigrationsAssembly(GeneratePathMigrations<TDbContext>(migrationsAssembly: migrationsAssembly));
            });
        });

        return services;
    }

    private static string GeneratePathMigrations<TDbContext>(string migrationsAssembly) where TDbContext : DbContext
    {
        string result;

        //if ((migrationsAssembly == null) || (migrationsAssembly == string.Empty))
        if (migrationsAssembly == string.Empty)
        {
            result = typeof(TDbContext).Assembly.FullName;
        }
        else
        {
            result = migrationsAssembly;
        }

        return result;
    }

    #endregion

    #region "Health Checks with UI"
    public static IServiceCollection AddHealthChecksUISQLite<TDbContext>(this IServiceCollection services, string webAddressTitle, string dbConnectionString) where TDbContext : DbContext
    {
        services.AddHealthChecks()
            .AddDbContextCheck<TDbContext>(name: "Application DB Context", failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(new Uri("https://angelodotnet.github.io/"), name: webAddressTitle, failureStatus: HealthStatus.Degraded)
            .AddSqlite(dbConnectionString);

        services.AddHealthChecksUI(setupSettings: setup =>
        {
            setup.AddHealthCheckEndpoint("Health Check", $"/healthz");
        }).AddInMemoryStorage();

        return services;
    }

    public static IServiceCollection AddHealthChecksUISQLServer<TDbContext>(this IServiceCollection services, string webAddressTitle, string dbConnectionString) where TDbContext : DbContext
    {
        services.AddHealthChecks()
            .AddDbContextCheck<TDbContext>(name: "Application DB Context", failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(new Uri("https://angelodotnet.github.io/"), name: webAddressTitle, failureStatus: HealthStatus.Degraded)
            .AddSqlServer(dbConnectionString);

        services.AddHealthChecksUI(setupSettings: setup =>
        {
            setup.AddHealthCheckEndpoint("Health Check", $"/healthz");
        }).AddInMemoryStorage();

        return services;
    }

    public static IServiceCollection AddHealthChecksUIMySQL<TDbContext>(this IServiceCollection services, string webAddressTitle, string dbConnectionString) where TDbContext : DbContext
    {
        services.AddHealthChecks()
            .AddDbContextCheck<TDbContext>(name: "Application DB Context", failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(new Uri("https://angelodotnet.github.io/"), name: webAddressTitle, failureStatus: HealthStatus.Degraded)
            .AddMySql(dbConnectionString);

        services.AddHealthChecksUI(setupSettings: setup =>
        {
            setup.AddHealthCheckEndpoint("Health Check", $"/healthz");
        }).AddInMemoryStorage();

        return services;
    }

    public static IServiceCollection AddHealthChecksUIPostgreSQL<TDbContext>(this IServiceCollection services, string webAddressTitle, string dbConnectionString) where TDbContext : DbContext
    {
        services.AddHealthChecks()
            .AddDbContextCheck<TDbContext>(name: "Application DB Context", failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(new Uri("https://angelodotnet.github.io/"), name: webAddressTitle, failureStatus: HealthStatus.Degraded)
            .AddNpgSql(dbConnectionString);

        services.AddHealthChecksUI(setupSettings: setup =>
        {
            setup.AddHealthCheckEndpoint("Health Check", $"/healthz");
        }).AddInMemoryStorage();

        return services;
    }

    public static WebApplication UseHealthChecksUI(this WebApplication app)
    {
        app.UseHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            },
        }).UseHealthChecksUI(setup =>
        {
            setup.ApiPath = "/healthcheck";
            setup.UIPath = "/healthcheck-ui";

            //https://github.com/Amitpnk/Onion-architecture-ASP.NET-Core/blob/develop/src/OA/Customization/custom.css
            //setup.AddCustomStylesheet("Customization/custom.css");
        });

        return app;
    }
    #endregion
}