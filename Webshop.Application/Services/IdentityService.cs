using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Webshop.Application.Results;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IdentityService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityRegisterResult> RegisterAsync(string email, string password)
        {
            var result = new IdentityRegisterResult();

            
            var user = new AppUser
            {
                UserName = email,
                Email = email
            };

            
            var identityResult = await _userManager.CreateAsync(user, password);
            result.IdentityResult = identityResult;

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    result.Failed(error.Description);
                }
            }

            return result;
        }
        public async Task<bool> IsInRoleAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;
            return await _userManager.IsInRoleAsync(user, roleName);
        }


        public async Task<IdentitySignInResult> SignInAsync(string email, string password)
        {
            var result = new IdentitySignInResult();

            
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                result.Failed("Invalid email or password.");
                return result;
            }

            
            var signInResult = await _signInManager.PasswordSignInAsync(
                user,
                password,
                isPersistent: false,
                lockoutOnFailure: false);

            result.SignInResult = signInResult;

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    result.Failed("This account has been locked out. Please try again later.");
                }
                else
                {
                    result.Failed("Invalid email or password.");
                }
            }

            return result;
        }

        public async Task<string?> EnsureExternalUserAsync(ClaimsPrincipal principal)
        {
            var email = principal.FindFirstValue(ClaimTypes.Email) ?? principal.FindFirstValue("email") ?? principal.Identity?.Name;
            var sub = principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? principal.FindFirstValue("sub");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sub))
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new AppUser
                    {
                        Id = sub,
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };
                    await _userManager.CreateAsync(user);
                }
                else
                {
                    // If user exists by email but has a different ID, we should ideally link them.
                    // For now, we return the existing user's ID to ensure they can see their orders if they previously registered.
                    return user.Id;
                }
            }

            return user.Id;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public AuthenticationProperties GoogleLogin(string redirectUrl)
        {
            string scheme = "google";
            return _signInManager.ConfigureExternalAuthenticationProperties(scheme, redirectUrl);
        }

        public async Task<GoogleResponseResult> GoogleResponse()
        {
            var result = new GoogleResponseResult();
            ExternalLoginInfo? externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                result.Failed("no external login information received.");
                return result;
            }

            var user = await _userManager.FindByLoginAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey);
            if (user == null)
            {
                user = await CreateIdentityUserFromClaims(externalLoginInfo);
            }

            if (user == null)
            {
                result.Failed("Could not find or create user");
                return result;
            }

            await _signInManager.SignInAsync(user, true);
            return result;
        }

        private async Task<AppUser?> CreateIdentityUserFromClaims(ExternalLoginInfo externalLoginInfo)
        {
            var claim = externalLoginInfo.Principal.FindFirst(ClaimTypes.Email);
            if (claim == null) return null;

            var email = claim.Value;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser {UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return null;
                }
            }
            var loginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
            if (!loginResult.Succeeded) return null;
            return user;
        }
    }
}
