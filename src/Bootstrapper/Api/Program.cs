using Keycloak.AuthServices.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;
var orderAssembly = typeof(OrderModule).Assembly;

builder.Services.AddCarterWithAssemblies(catalogAssembly, basketAssembly, orderAssembly);
builder.Services.AddMediatorAssemblies(catalogAssembly, basketAssembly, orderAssembly);
builder.Services.AddMassTransitWithAssemblies(
    builder.Configuration, catalogAssembly, basketAssembly);
builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

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
app.UseAuthentication();
app.UseAuthorization();

await app.UseCatalogModule();
await app.UseBasketModule();
await app.UseOrderModule();

await app.RunAsync();
