using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomCoreIdentity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CustomCoreIdentity.Data {
    public class AppUserManager : UserManager<AppUser> {
        private readonly RoleManager<UserRole> _roleManager;

        public AppUserManager(IUserStore<AppUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<AppUser> passwordHasher,
            IEnumerable<IUserValidator<AppUser>> userValidators,
            IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<UserManager<AppUser>> logger,
            RoleManager<UserRole> roleManager)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors,
                services, logger) {
            _roleManager = roleManager;
        }

        public async Task<IList<UserRole>> GetModelRolesAsync(AppUser user)
        {
            IList<string> roleNames = await base.GetRolesAsync(user);

            var identityRoles = new List<UserRole>();
            foreach (var roleName in roleNames)
            {
                UserRole role = await _roleManager.FindByNameAsync(roleName);
                identityRoles.Add(role);
            }

            return identityRoles;
        }
    }
}
