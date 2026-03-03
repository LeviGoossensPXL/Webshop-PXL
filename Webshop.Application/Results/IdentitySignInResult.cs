using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Application.Results
{
    public class IdentitySignInResult : BaseResult
    {
        public SignInResult SignInResult { get; set; }
    }
}
