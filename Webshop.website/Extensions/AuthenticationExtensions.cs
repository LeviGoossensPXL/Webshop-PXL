using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Webshop.website.Extensions;

public static class AuthenticationExtensions
{
    public static void AddProjectAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var googleAuth = configuration.GetSection("Authentication:Google");

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddGoogle("google", options =>
            {
                options.ClientId = googleAuth["ClientId"]!;
                options.ClientSecret = googleAuth["ClientSecret"]!;
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Identity.Application";
                options.Authority = "https://localhost:5001";

                options.ClientId = "webshop_client";
                options.ClientSecret = "super_secret_webshop_key";
                options.ResponseType = "code";
                options.SaveTokens = true;

                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("roles");

                options.GetClaimsFromUserInfoEndpoint = true;
                options.RequireHttpsMetadata = false;
                options.MapInboundClaims = false;
                options.ClaimActions.MapJsonKey("role", "role", "role");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "email",
                    RoleClaimType = "role"
                };
            });
    }
}