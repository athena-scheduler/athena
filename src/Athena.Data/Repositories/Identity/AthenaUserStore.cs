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
    public partial class AthenaUserStore : IUserLoginStore<AthenaUser>, IUserEmailStore<AthenaUser>
    {
        private readonly IDbConnection _db;
        private readonly ILogger _log = Log.ForContext<AthenaUserStore>();

        public AthenaUserStore(IDbConnection db) => _db = db ?? throw new ArgumentNullException(nameof(db));
        
        public void Dispose()
        {
            // _db disposed by IOC / DI
        }

        public Task<string> GetUserIdAsync(AthenaUser user, CancellationToken cancellationToken) =>
            Task.FromResult(user.Id.ToString());

        public Task<string> GetUserNameAsync(AthenaUser user, CancellationToken cancellationToken) =>
            Task.FromResult(user.UserName);

        public Task SetUserNameAsync(AthenaUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.UserName = userName;

            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(AthenaUser user, CancellationToken cancellationToken) =>
            Task.FromResult(user.NormalizedUserName);

        public Task SetNormalizedUserNameAsync(AthenaUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(AthenaUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user.Id == Guid.Empty)
            {
                _log.Fatal("This can't possibly work. Tried to add {@user} but the ID is an empty guid. This must match the ID of a student", user);
                return IdentityResult.Failed();
            }

            try
            {
                var count = await _db.InsertUniqueAsync(@"
                    INSERT INTO users VALUES (
                        @id,
                        @username,
                        @normalizedUsername,
                        @email,
                        @normalizedEmail,
                        @emailConfirmed
                    )",
                    new { user.Id, user.UserName, user.NormalizedUserName, user.Email, user.NormalizedEmail, user.EmailConfirmed }
                );

                return count == 1 ? IdentityResult.Success : IdentityResult.Failed();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Failed to create user {@user}", user);
                return IdentityResult.Failed();
            }

        }

        public async Task<IdentityResult> UpdateAsync(AthenaUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var count = await _db.ExecuteAsync(@"
                    UPDATE users SET username = @username,
                                     normalized_username = @normalizedUsername,
                                     email = @email,
                                     normalized_email = @normalizedEmail,
                                     email_confirmed = @emailConfirmed
                    WHERE id = @id",
                    new {user.UserName, user.NormalizedUserName, user.Email, user.NormalizedEmail, user.EmailConfirmed, user.Id}
                );
                
                return count == 1 ? IdentityResult.Success : IdentityResult.Failed();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Failed to update user {@user}", user);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> DeleteAsync(AthenaUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var count = await _db.ExecuteAsync(
                    "DELETE FROM users WHERE id = @id",
                    new {user.Id}
                );
                
                return count == 1 ? IdentityResult.Success : IdentityResult.Failed();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Failed to delete user {@user}", user);
                return IdentityResult.Failed();
            }
        }

        public async Task<AthenaUser> FindByIdAsync(string userId, CancellationToken cancellationToken) =>
            (await _db.QueryAsync<AthenaUser>(
                "SELECT * FROM users WHERE id = @id",
                new {id = new Guid(userId)}
            )).FirstOrDefault();

        public async Task<AthenaUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) =>
            (await _db.QueryAsync<AthenaUser>(
                "SELECT * FROM users WHERE normalized_username = @normalizedUserName",
                new {normalizedUserName}
            )).FirstOrDefault();
    }
}