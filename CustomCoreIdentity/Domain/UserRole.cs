using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CustomCoreIdentity.Domain
{
    public class UserRole : IdentityRole
    {
        public UserRole() : base()
        {
        }
        // Add custom columns here
        public UserRole(string roleName) : base(roleName)
        {
        }

    }
}
