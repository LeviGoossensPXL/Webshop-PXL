using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Security.Claims;

namespace DuendeIdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                // define a new resource for roles so duende knows it can share role claims
                new IdentityResource("roles", "User Roles", new[] { "role" })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
                { };

        public static IEnumerable<Client> GetClients(ConfigurationManager configuration) {
            return new Client[]
                { 
                    new Client
                    {
                        ClientId = configuration["Auth:Duende:ClientId"]!,
                        ClientSecrets = { new Secret(configuration["Auth:Duende:ClientSecret"]!.Sha256()) },
                        AllowedGrantTypes = GrantTypes.Code,
                    
                        // require Proof Key for Code Exchange (PKCE) for maximum security standard
                        RequirePkce = true,
                        // allow the application to request refresh tokens to keep users logged in
                        AllowOfflineAccess = true,

                        RedirectUris = { $"{configuration["WebsiteHost"]}/signin-oidc" },
                        PostLogoutRedirectUris = { $"{configuration["WebsiteHost"]}/signout-callback-oidc" },

                        AllowedScopes = { "openid", "profile", "email", "roles" },
                        AlwaysIncludeUserClaimsInIdToken = true
                    }
                };
        }
    }
}