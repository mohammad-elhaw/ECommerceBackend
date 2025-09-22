using Basket;
using Carter;
using Catalog;
using Order;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCarterWithAssemblies();
builder.Services.AddMediatorAssemblies();

builder.Services.AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderModule(builder.Configuration);

var app = builder.Build();

// Configure Http request pipeline.

app.MapCarter();

await app.UseCatalogModule();
    app.UseBasketModule()
    .UseOrderModule();

app.Run();
