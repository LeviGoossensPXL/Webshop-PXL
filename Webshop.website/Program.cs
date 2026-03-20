using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;
using Webshop.Infrastructure.Data;
using Webshop.Infrastructure.Repositories;
using Webshop.website.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddControllersWithViews();

// Register application services and repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IStockItemRepository, StockItemRepository>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Session configuration
builder.Services.AddDistributedMemoryCache(); // Vereist voor session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Winkelmandje blijft 30 min bewaard
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor(); // Voor toegang tot HttpContext in services

var app = builder.Build();

// ==========================================================
using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedAsync(scope.ServiceProvider);
}
// ==========================================================

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // MOET V””R UseAuthorization staan
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
