var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCarterWithAssemblies();
builder.Services.AddMediatorAssemblies();

builder.Services.AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure Http request pipeline.

app.UseExceptionHandler(options => { });

app.MapCarter();

await app.UseCatalogModule();
    app.UseBasketModule()
    .UseOrderModule();

app.Run();
