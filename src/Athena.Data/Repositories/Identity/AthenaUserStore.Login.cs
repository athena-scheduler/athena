using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using Athena.Data.Extensions;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace Athena.Data.Repositories.Identity
{
    public partial class AthenaUserStore : IUserLoginStore<AthenaUser>, IUserEmailStore<AthenaUser>
    {
        private class UserLoginInfoEx
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string ProviderDisplayName { get; set; }
        }
        
        public async Task AddLoginAsync(AthenaUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _db.InsertUniqueAsync(
                    "INSERT INTO external_logins VALUES (@providerKey, @loginProvider, @providerDisplayName, @userId)",
                    new { login.ProviderKey, login.LoginProvider, login.ProviderDisplayName, userId = user.Id }
                );
                
                _log.Debug("External login {@login} added for {@user}", login, user);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "Failed to add external login {@login} for user {@user}", login, user);
                throw;
            }
        }

        public async Task RemoveLoginAsync(AthenaUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _db.ExecuteAsync(@"
                    DELETE FROM external_logins WHERE login_provider = @loginProvider AND
                                                      provider_key = @providerKey AND
                                                      user_id = @userId",
                    new { loginProvider, providerKey, userId = user.Id }
                );
                
                _log.Debug("External login from {loginProvider} removed for {@user}", loginProvider, user);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "Failed to remove external login from {loginProvider} for {@user}", loginProvider, user);
                throw;
            }
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(AthenaUser user, CancellationToken cancellationToken) =>
            (await _db.QueryAsync<UserLoginInfoEx>(@"
                SELECT login_provider,
                       provider_key,
                       provider_display_name
                FROM external_logins
                WHERE user_id = @id",
                new {user.Id}
            )).Select((e) => new UserLoginInfo(e.LoginProvider, e.ProviderKey, e.ProviderDisplayName))
              .ToList();

        public async Task<AthenaUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken) =>
            (await _db.QueryAsync<AthenaUser, Student, AthenaUser>(@"
                SELECT u.id,
                       u.username,
                       u.normalized_username,
                       u.email,
                       u.normalized_email,
                       u.email_confirmed,
                       s.id,
                       s.name,
                       s.email
                FROM users u
                    LEFT JOIN students s ON u.id = s.id
                    LEFT JOIN external_logins link ON u.id = link.user_id
                WHERE link.login_provider = @loginProvider AND link.provider_key = @providerKey",
                MapUser,
                new {loginProvider, providerKey}
            )).FirstOrDefault();
    }
}