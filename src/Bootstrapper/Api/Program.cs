using Microsoft.Extensions.Caching.Hybrid;

var builder = WebApplication.CreateBuilder(args);

builder.AddSeqEndpoint(connectionName: "seq");

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.AddServiceDefaults();
builder.Services.AddOpenApi();

#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(5),
        Expiration = TimeSpan.FromMinutes(10),
        
    };
});
#pragma warning restore EXTEXP0018

builder.AddRedisDistributedCache("redis");

var catalogAssemblies = typeof(CatalogModule).Assembly;
var basketAssemblies = typeof(BasketModule).Assembly;

builder.Services.AddCarterWithAssemblies(
   catalogAssemblies,
   basketAssemblies);

builder.Services.AddMediatRWithAssemblies(catalogAssemblies, basketAssemblies);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });

    app.MapScalarApiReference(o => o.WithTheme(ScalarTheme.DeepSpace));
}

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler("/error");

app.UseCatalogModule()
   .UseBasketModule()
   .UseOrderingModule();

app.Run();