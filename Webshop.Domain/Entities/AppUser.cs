using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Webshop.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public int AppUserId { get; set; }
        public string UserId { get; set; } // id from IdentityUser
    }
}
