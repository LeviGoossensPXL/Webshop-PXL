using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Results;

namespace Webshop.Application.Services.Contracts
{
    public interface IIdentityService
    {
        Task<IdentitySignInResult> SignInAsync(string email, string password);

        Task SignOutAsync();

        Task<IdentityRegisterResult> RegisterAsync(string email, string password);
    }
}
