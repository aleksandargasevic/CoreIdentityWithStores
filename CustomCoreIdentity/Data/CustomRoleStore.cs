using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CustomCoreIdentity.Domain;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomCoreIdentity.Data
{
    public class CustomRoleStore : IRoleStore<UserRole>, IRoleClaimStore<UserRole>
    {
        private readonly ApplicationDbContext _context;
        public IdentityErrorDescriber ErrorDescriber { get; set; }
        public bool AutoSaveChanges { get; set; } = true;
        private bool _disposed;

        public CustomRoleStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) {
            _context = context;
            ErrorDescriber = describer ?? new IdentityErrorDescriber();
        }

        private async Task SaveChanges(CancellationToken cancellationToken)
        {
            if (AutoSaveChanges)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public void Dispose() => _disposed = true;

        public async Task<IdentityResult> CreateAsync(UserRole role, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _context.Roles.Add(role);
            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(UserRole role, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _context.Roles.Attach(role);
            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            _context.Roles.Update(role);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(UserRole role, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _context.Roles.Remove(role);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(UserRole role, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(UserRole role, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(UserRole role, string roleName, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(UserRole role, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(UserRole role, string normalizedName, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task<UserRole> FindByIdAsync(string roleId, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _context.Roles.FirstOrDefaultAsync(u => u.Id.Equals(roleId), cancellationToken);
        }

        public Task<UserRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return _context.Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public async Task<IList<Claim>> GetClaimsAsync(UserRole role, CancellationToken cancellationToken = new CancellationToken()) {
            if (role == null)
                throw new ArgumentNullException("role");
            return await _context.RoleClaims.Where(c => c.RoleId == role.Id).Select(c => c.ToClaim()).ToListAsync(cancellationToken);
        }

        public async Task AddClaimAsync(UserRole role, Claim claim, CancellationToken cancellationToken = new CancellationToken()) {
            if (role == null)
                throw new ArgumentNullException("role");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var instance = Activator.CreateInstance<IdentityRoleClaim<string>>();
            instance.RoleId = role.Id;
            instance.ClaimType = claim.Type;
            instance.ClaimValue = claim.Value;
            var roleClaim = instance;
            _context.RoleClaims.Add(roleClaim);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveClaimAsync(UserRole role, Claim claim, CancellationToken cancellationToken = new CancellationToken()) {
            if (role == null)
                throw new ArgumentNullException("role");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var claimToRemove = await _context.RoleClaims.SingleOrDefaultAsync(c =>
                c.RoleId == role.Id && c.ClaimValue == claim.Value && c.ClaimType == claim.Type, cancellationToken);
            _context.RoleClaims.Remove(claimToRemove);
        }
    }
}
