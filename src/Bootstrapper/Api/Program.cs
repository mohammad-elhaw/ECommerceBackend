var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;

builder.Services.AddCarterWithAssemblies(catalogAssembly, basketAssembly);
builder.Services.AddMediatorAssemblies(catalogAssembly, basketAssembly);
builder.Services.AddMassTransitWithAssemblies(
    builder.Configuration, catalogAssembly, basketAssembly);

builder.Services.AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderModule(builder.Configuration);

builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.Configuration = builder.Configuration.GetConnectionString("redis");
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure Http request pipeline.

app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseSerilogRequestLogging();

await app.UseCatalogModule();
await app.UseBasketModule();
app.UseOrderModule();

await app.RunAsync();
