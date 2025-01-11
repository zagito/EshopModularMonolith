using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Interceptors;

namespace Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
        services.Decorate<IShoppingCartRepository, CachedShoppingCartRepository>();

        //services.AddScoped<IShoppingCartRepository>(provider =>
        //{
        //    var repository = provider.GetRequiredService<ShoppingCartRepository>();
        //    return new CachedShoppingCartRepository(repository, provider.GetRequiredService<HybridCache>());
        //});

        var connectionString = configuration.GetConnectionString("eshopdb");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<BasketDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        app.UseMigrations<BasketDbContext>();

        return app;
    }
}
