using CustomLibrary.EFCore.EFCore.Infrastructure.Interfaces;
using CustomLibrary.EFCore.EFCore.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CustomLibrary.EFCore.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDbContextGenerics<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
    {
        services.AddScoped<DbContext, TDbContext>();
        services.AddScoped(typeof(IUnitOfWork<,>), typeof(UnitOfWork<,>));
        services.AddScoped(typeof(IDatabaseRepository<,>), typeof(DatabaseRepository<,>));
        services.AddScoped(typeof(ICommandRepository<,>), typeof(CommandRepository<,>));

        return services;
    }

    //public static IServiceCollection AddDbContextTransaction<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
    //{
    //    services.AddScoped<DbContext, TDbContext>();
    //    services.AddScoped(typeof(ITUnitOfWork<,>), typeof(TUnitOfWork<,>));
    //    services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

    //    return services;
    //}

    public static IServiceCollection AddDbContextForMySql<TDbContext>(this IServiceCollection services, string connectionString, int retryOnFailure, string migrationsAssembly) where TDbContext : DbContext
    {
        if ((migrationsAssembly == null) || (migrationsAssembly == string.Empty))
        {
            migrationsAssembly = typeof(TDbContext).Assembly.FullName;
        }

        services.AddDbContextPool<TDbContext>(optionBuilder =>
        {
            if (retryOnFailure > 0)
            {
                optionBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
                {
                    // Abilito il connection resiliency (Provider di Mysql / MariaDB è soggetto a errori transienti)
                    // Info su: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                    options.EnableRetryOnFailure(retryOnFailure);
                    //options.MigrationsAssembly(typeof(TDbContext).Assembly.FullName);
                    options.MigrationsAssembly(migrationsAssembly);
                });
            }
            else
            {
                optionBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
                {
                    //options.MigrationsAssembly(typeof(TDbContext).Assembly.FullName);
                    options.MigrationsAssembly(migrationsAssembly);
                });
            }
        });
        return services;
    }

    public static IServiceCollection AddDbContextUsePostgres<TDbContext>(this IServiceCollection services, string connectionString, int retryOnFailure, string migrationsAssembly) where TDbContext : DbContext
    {
        if ((migrationsAssembly == null) || (migrationsAssembly == string.Empty))
        {
            migrationsAssembly = typeof(TDbContext).Assembly.FullName;
        }

        services.AddDbContextPool<TDbContext>(optionBuilder =>
        {
            if (retryOnFailure > 0)
            {
                optionBuilder.UseNpgsql(connectionString, options =>
                {
                    // Abilito il connection resiliency (Provider di Postgres è soggetto a errori transienti)
                    // Info su: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                    options.EnableRetryOnFailure(retryOnFailure);
                    //options.MigrationsAssembly(typeof(TDbContext).Assembly.FullName);
                    options.MigrationsAssembly(migrationsAssembly);
                });
            }
            else
            {
                optionBuilder.UseNpgsql(connectionString, options =>
                {
                    //options.MigrationsAssembly(typeof(TDbContext).Assembly.FullName);
                    options.MigrationsAssembly(migrationsAssembly);
                });
            }
        });

        return services;
    }

    public static IServiceCollection AddDbContextUseSQLServer<TDbContext>(this IServiceCollection services, string connectionString, int retryOnFailure, string migrationsAssembly) where TDbContext : DbContext
    {
        if ((migrationsAssembly == null) || (migrationsAssembly == string.Empty))
        {
            migrationsAssembly = typeof(TDbContext).Assembly.FullName;
        }

        services.AddDbContextPool<TDbContext>(optionBuilder =>
        {
            if (retryOnFailure > 0)
            {
                optionBuilder.UseSqlServer(connectionString, options =>
                {
                    // Abilito il connection resiliency (Provider di SQL Server è soggetto a errori transienti)
                    // Info su: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                    options.EnableRetryOnFailure(retryOnFailure);
                    //options.MigrationsAssembly(typeof(TDbContext).Assembly.FullName);
                    options.MigrationsAssembly(migrationsAssembly);
                });
            }
            else
            {
                optionBuilder.UseSqlServer(connectionString, options =>
                {
                    //options.MigrationsAssembly(typeof(TDbContext).Assembly.FullName);
                    options.MigrationsAssembly(migrationsAssembly);
                });
            }
        });

        return services;
    }

    public static IServiceCollection AddDbContextUseSQLite<TDbContext>(this IServiceCollection services, string connectionString, string migrationsAssembly) where TDbContext : DbContext
    {
        if ((migrationsAssembly == null) || (migrationsAssembly == string.Empty))
        {
            migrationsAssembly = typeof(TDbContext).Assembly.FullName;
        }

        services.AddDbContextPool<TDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlite(connectionString, options =>
            {
                // Non abilito il connection resiliency, non è supportato dal provider di Sqlite in quanto non soggetto a errori transienti)
                //options.MigrationsAssembly(typeof(TDbContext).Assembly.FullName);
                options.MigrationsAssembly(migrationsAssembly);
            });
        });

        return services;
    }
}