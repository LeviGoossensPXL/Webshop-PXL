using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Webshop.Application.Results
{
    public class IdentityRegisterResult : BaseResult
    {
        IdentityResult IdentityResult { get; set; }
    }
}
