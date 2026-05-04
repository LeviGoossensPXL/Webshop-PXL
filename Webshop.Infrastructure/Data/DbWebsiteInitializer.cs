using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;

namespace Webshop.Infrastructure.Data;

public static class DbWebsiteInitializer
{
    /// <summary>
    /// seed-data for proper function of the app
    /// </summary>
    /// <param name="serviceProvider">used to access registered services</param>
    public static async Task InitAsync(IServiceProvider serviceProvider)
    {
        await SeedRolesAsync(serviceProvider);
        await SeedUsersAsync(serviceProvider);

        var categoryRepo = serviceProvider.GetRequiredService<ICategoryRepository>();
        // 1. Create 6 main camping categories
        var existingCategories = await categoryRepo.GetAll();
        if (!existingCategories.Any())
        {
            foreach (var category in SeedData.Categories)
            {
                await categoryRepo.Add(category);
            }
        }
    }

    /// <summary>
    /// seed-data not needed for proper function, but used for testing, displaying, ...
    /// </summary>
    /// <param name="serviceProvider">used to access registered services</param>
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        await SeedRolesAsync(serviceProvider);

        var productRepo = serviceProvider.GetRequiredService<IProductRepository>();

        var existingProducts = await productRepo.GetAll();
        if (existingProducts.Any())
        {
            return; // Stop here if products already exist in database
        }

        foreach (var product in SeedData.Products)
        {
            await productRepo.Add(product);
        }
    }

    /// <summary>
    /// if roles do not exist we add them here
    /// </summary>
    /// <param name="serviceProvider"></param>
    private static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in SeedData.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    /// <summary>
    /// add initial users
    /// </summary>
    /// <param name="serviceProvider"></param>
    private static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        if (await userManager.FindByEmailAsync(SeedData.AdminUser.Email!) != null)
        {
            return;
        }

        string adminPassword = "AdminPassword123!";
        var result = await userManager.CreateAsync(SeedData.AdminUser, adminPassword);
        if (result.Succeeded)
        {
            var createdUser = await userManager.FindByEmailAsync(SeedData.AdminUser.Email!);
            await userManager.AddToRoleAsync(createdUser!, "Admin");
        }
    }
}