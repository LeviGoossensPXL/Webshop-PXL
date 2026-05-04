using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;

namespace Webshop.Infrastructure.Data;

public static class DbApiInitializer
{
    /// <summary>
    /// seed-data not needed for proper function, but used for testing, displaying, ...
    /// </summary>
    /// <param name="serviceProvider">used to access registered services</param>
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var stockItemRepo = serviceProvider.GetRequiredService<IStockItemRepository>();

        var existingStocks = await stockItemRepo.GetAll();
        if (existingStocks.Any())
        {
            return; // Stop here if stocks already exist in database
        }

        foreach (var stockItem in SeedData.StockItems)
        {
            await stockItemRepo.Add(stockItem);
        }
    }
}