using Microsoft.AspNetCore.Authentication;
using DotNetEnv.Configuration;
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
// Load environment variables from the .env file
builder.Configuration.AddDotNetEnv("../");

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

// add openid connect scheme alongside the existing local identity schemes
builder.Services.AddAuthentication()
    .AddOpenIdConnect("oidc", options =>
    {
        options.SignInScheme = "Identity.Application";
        // point to the duende identity server container/localhost
        options.Authority = "https://localhost:5001";

        // credentials must match the config.cs in the duende project
        options.ClientId = "webshop_client";
        options.ClientSecret = "super_secret_webshop_key";
        options.ResponseType = "code";
        options.SaveTokens = true;

        // request standard user data scopes
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("roles");

        // fetch additional claims to automatically populate the user identity
        options.GetClaimsFromUserInfoEndpoint = true;

        // prevent https certificate validation errors in local docker dev environments
        options.RequireHttpsMetadata = false;

        options.MapInboundClaims = false;
        options.ClaimActions.MapJsonKey("role", "role", "role");

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            NameClaimType = "email",
            RoleClaimType = "role"
        };
    });

// Register application services and repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Session configuration
builder.Services.AddDistributedMemoryCache(); // Vereist voor session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Winkelmandje blijft 30 min bewaard
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor(); // Voor toegang tot HttpContext in services
var authGoogle = builder.Configuration.GetSection("Authentication:Google");
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddGoogle("google", options =>
    {
        options.ClientId = authGoogle["ClientId"]!;
        options.ClientSecret = authGoogle["ClientSecret"]!;
    });
// Named HttpClient for Stock Web API
builder.Services.AddHttpClient("StockApi", client =>
{
    // In Docker: http://webshop.webapi:8080, locally: http://localhost:8078
    var stockApiUrl = builder.Configuration["StockApi:BaseUrl"] ?? "http://localhost:8078";
    client.BaseAddress = new Uri(stockApiUrl);
});

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
    app.UseHsts();
    app.UseHttpsRedirection();
}

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
