using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Webshop.Application.Repositories;
using Webshop.Application.Services;
using Webshop.Application.Services.Contracts;
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

// Add Identity setup for Users and Roles
builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor(); // Enable Blazor Server services 



// Register application services and repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IStockItemRepository, StockItemRepository>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();

// Session configuration
builder.Services.AddDistributedMemoryCache(); // Vereist voor session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Winkelmandje blijft 30 min bewaard
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor(); // Voor toegang tot HttpContext in services
builder.Services.AddHttpClient(); // Voor externe API-aanroepen in services(Prepare for the API stock creation requirement)

var app = builder.Build();

// ==========================================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); // always apply migrations during startup (could cause problems)
    // more info here: https://codebuckets.com/2020/08/14/applying-entity-framework-migrations-to-a-docker-container/

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
app.UseSession(); // MOET V��R UseAuthorization staan
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub(); // Map Blazor Server Hub (Open the connection for Blazor Server communication)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shop}/{action=Index}/{id?}");

app.Run();
