using Basket;
using Catalog;
using Order;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderModule(builder.Configuration);

var app = builder.Build();

// Configure Http request pipeline.
app.UseCatalogModule()
    .UseBasketModule()
    .UseOrderModule();

app.Run();
