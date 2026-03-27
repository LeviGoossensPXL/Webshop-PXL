using Microsoft.AspNetCore.Identity;
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

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
