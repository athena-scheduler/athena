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
    public partial class AthenaUserStore : IUserLoginStore<AthenaUser>, IUserEmailStore<AthenaUser>, IUserRoleStore<AthenaUser>, IUserApiKeyStore
    {
        public async Task AddToRoleAsync(AthenaUser user, string roleName, CancellationToken cancellationToken) =>
            await _db.InsertUniqueAsync(@"
                INSERT INTO user_x_role
                SELECT
                    @id,
                    r.id
                FROM roles r
                WHERE r.normalized_name = @name",
                new { id = user.Id, name = roleName }
            );

        public async Task RemoveFromRoleAsync(AthenaUser user, string roleName, CancellationToken cancellationToken) =>
            await _db.ExecuteAsync(@"
                DELETE FROM user_x_role
                WHERE user_id = @id AND role_id IN
                (
                    SELECT r.id
                    FROM roles r
                    WHERE r.normalized_name = @roleName
                )",
                new {user.Id, roleName}
            );

        public async Task<IList<string>> GetRolesAsync(AthenaUser user, CancellationToken cancellationToken) =>
            (await _db.QueryAsync<string>(@"
                SELECT r.normalized_name
                FROM roles r
                    LEFT JOIN user_x_role link ON r.id = link.role_id
                WHERE link.user_id = @id",
                new {user.Id}
            )).ToList();

        public async Task<bool> IsInRoleAsync(AthenaUser user, string roleName, CancellationToken cancellationToken) =>
            (await _db.QueryAsync(@"
                SELECT link.user_id
                FROM user_x_role link
                    LEFT JOIN roles r ON link.role_id = r.id
                WHERE link.user_id = @id",
                new {user.Id}
            )).Any();

        public async Task<IList<AthenaUser>>
            GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken) =>
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
                    LEFT JOIN user_x_role link ON u.id = link.user_id
                    LEFT JOIN roles r ON link.role_id = r.id
                WHERE r.normalized_name = @roleName",
                MapUser,
                new {roleName}
            )).ToList();
    }
}