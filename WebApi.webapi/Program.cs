using Microsoft.EntityFrameworkCore;
using WebApi.Repositories;
using Webshop.Application.Repositories;
using Webshop.Application.Services;
using Webshop.Application.Services.Contracts;
using Webshop.Infrastructure.Data;
using AppDbContext = WebApi.Data.AppDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("StockWebApiConnection"));
});
builder.Services.AddScoped<IStockItemService, StockItemService>();
builder.Services.AddScoped<IStockItemRepository, StockItemRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    await DbApiInitializer.SeedAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();


app.MapGet("/StockItem/summary", async (IStockItemService stockItemService) =>
{
    var result = await stockItemService.GetAll();

    if (result.Succeeded && result.Data != null)
    {
        // Calculate a quick summary of the warehouse
        var totalUniqueProducts = result.Data.Count();
        var totalItemsInStock = result.Data.Sum(s => s.Quantity);

        // Return a clean anonymous object with a 200 OK status
        return Results.Ok(new
        {
            TotalUniqueProducts = totalUniqueProducts,
            TotalItemsInStock = totalItemsInStock,
            Message = "Warehouse summary generated via Minimal API"
        });
    }

    return Results.Problem("Could not retrieve stock summary.");
})
.WithName("GetStockSummary");

app.Run();

