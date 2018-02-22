using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Models.Identity;
using Athena.Data.Extensions;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Athena.Data.Repositories.Identity
{
    public class AthenaRoleStore : IRoleStore<AthenaRole>
    {
        private readonly IDbConnection _db;
        private readonly ILogger _log = Log.ForContext<AthenaRoleStore>();

        public AthenaRoleStore(IDbConnection db) => _db = db ?? throw new ArgumentNullException(nameof(db));
        
        public void Dispose()
        {
            // _db disposed by IOC Framework
        }

        public async Task<IdentityResult> CreateAsync(AthenaRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            role.Id = role.Id == Guid.Empty ? Guid.NewGuid() : role.Id;
            
            try
            {
                var count = await _db.InsertUniqueAsync(@"
                    INSERT INTO roles VALUES (
                        @id,
                        @name,
                        @normalizedName
                    )",
                    new {role.Id, role.Name, role.NormalizedName}
                );

                return count == 1 ? IdentityResult.Success : IdentityResult.Failed();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Failed to create role {@role}", role);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> UpdateAsync(AthenaRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var count = await _db.ExecuteAsync(
                    "UPDATE roles SET name = @name, normalized_name = @normalizedName WHERE id = @id",
                    new { role.Name, role.NormalizedName, role.Id }
                );

                return count == 1 ? IdentityResult.Success : IdentityResult.Failed();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Failed to update role {@role}", role);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> DeleteAsync(AthenaRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var count = await _db.ExecuteAsync(
                    "DELETE FROM roles WHERE id = @id",
                    new {role.Id}
                );
                
                return count == 1 ? IdentityResult.Success : IdentityResult.Failed();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Failed to delete role {@role}", role);
                return IdentityResult.Failed();
            }
        }

        public Task<string> GetRoleIdAsync(AthenaRole role, CancellationToken cancellationToken) =>
            Task.FromResult(role.Id.ToString());

        public Task<string> GetRoleNameAsync(AthenaRole role, CancellationToken cancellationToken) =>
            Task.FromResult(role.Name);

        public Task SetRoleNameAsync(AthenaRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            role.Name = roleName;

            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(AthenaRole role, CancellationToken cancellationToken) =>
            Task.FromResult(role.NormalizedName);

        public Task SetNormalizedRoleNameAsync(AthenaRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            role.NormalizedName = normalizedName;

            return Task.CompletedTask;
        }

        public async Task<AthenaRole> FindByIdAsync(string roleId, CancellationToken cancellationToken) =>
            (await _db.QueryAsync<AthenaRole>("SELECT * FROM roles WHERE id = @id", new {id = new Guid(roleId)}))
                .FirstOrDefault();

        public async Task<AthenaRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) =>
            (await _db.QueryAsync<AthenaRole>(
                "SELECT * FROM roles WHERE normalized_name = @normalizedName",
                new {normalizedName = normalizedRoleName})
            ).FirstOrDefault();
    }
}