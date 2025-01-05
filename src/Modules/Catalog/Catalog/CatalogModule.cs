using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;
using Shared.Data.Interceptors;


namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
        });

        service.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        var connectionString = configuration.GetConnectionString("eshopdb");

        service.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        service.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        service.AddDbContext<CatalogDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        service.AddScoped<IDataSeeder, CatalogDataSeeder>();

        return service;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        app.UseMigrations<CatalogDbContext>();
        return app;
    }
}
