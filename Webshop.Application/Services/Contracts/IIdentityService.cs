using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Webshop.Application.Results;

namespace Webshop.Application.Services.Contracts
{
    public interface IIdentityService
    {
        Task<IdentitySignInResult> SignInAsync(string email, string password);

        Task SignOutAsync();

        Task<IdentityRegisterResult> RegisterAsync(string email, string password);
        public AuthenticationProperties GoogleLogin(string redirectUrl);
        public Task<GoogleResponseResult> GoogleResponse();
        Task<bool> IsInRoleAsync(string email, string roleName);
    }
}
