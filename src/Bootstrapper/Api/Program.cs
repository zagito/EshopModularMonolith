var builder = WebApplication.CreateBuilder(args);

builder.AddSeqEndpoint(connectionName: "seq");

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.AddServiceDefaults();
builder.Services.AddOpenApi();

builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

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