using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace Athena.Data.Repositories.Identity
{
    public partial class AthenaUserStore : IUserLoginStore<AthenaUser>, IUserEmailStore<AthenaUser>, IUserRoleStore<AthenaUser>, IUserApiKeyStore
    {
        public Task SetEmailAsync(AthenaUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.Email = email;

            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(AthenaUser user, CancellationToken cancellationToken) =>
            Task.FromResult(user.Email);

        public Task<bool> GetEmailConfirmedAsync(AthenaUser user, CancellationToken cancellationToken) =>
            Task.FromResult(user.EmailConfirmed);

        public Task SetEmailConfirmedAsync(AthenaUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.EmailConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public async Task<AthenaUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken) =>
            (await _db.QueryAsync<AthenaUser, Student, AthenaUser>(@"
                SELECT u.id,
                       u.username,
                       u.normalized_username,
                       u.email,
                       u.normalized_email,
                       u.email_confirmed,
                       u.api_key,
                       s.id,
                       s.name,
                       s.email
                FROM users u
                    LEFT JOIN students s ON u.id = s.id
                WHERE normalized_email = @normalizedEmail",
                MapUser,
                new {normalizedEmail}
            )).FirstOrDefault();

        public Task<string> GetNormalizedEmailAsync(AthenaUser user, CancellationToken cancellationToken) =>
            Task.FromResult(user.NormalizedEmail);

        public Task SetNormalizedEmailAsync(AthenaUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.NormalizedEmail = normalizedEmail;

            return Task.CompletedTask;
        }
    }
}