using System;
using System.Collections.Generic;
using System.Text;
using CustomCoreIdentity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomCoreIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,UserRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
