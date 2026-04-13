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

        public static IEnumerable<Client> Clients =>
            new Client[]
                { 
                    new Client
                    {
                        ClientId = "webshop_client",
                        ClientSecrets = { new Secret("super_secret_webshop_key".Sha256()) },
                        AllowedGrantTypes = GrantTypes.Code,
                    
                        // require Proof Key for Code Exchange (PKCE) for maximum security standard
                        RequirePkce = true,
                        // allow the application to request refresh tokens to keep users logged in
                        AllowOfflineAccess = true,

                        RedirectUris = { "https://localhost:7117/signin-oidc" },
                        PostLogoutRedirectUris = { "https://localhost:7117/signout-callback-oidc" },

                        AllowedScopes = { "openid", "profile", "email", "roles" },
                        AlwaysIncludeUserClaimsInIdToken = true
                    }
                };
    }
}