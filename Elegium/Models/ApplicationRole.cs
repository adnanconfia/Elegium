using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Elegium.Models
{
    public class ApplicationRole : IdentityRole<string>
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}