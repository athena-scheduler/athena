using System.Linq;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace Athena.Data.Repositories.Identity
{
    public partial class AthenaUserStore : IUserLoginStore<AthenaUser>, IUserEmailStore<AthenaUser>, IUserRoleStore<AthenaUser>, IUserApiKeyStore
    {
        public async Task<AthenaUser> GetUserForApiKey(string apiKey) =>
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
                WHERE u.api_key = @apiKey",
                MapUser,
                new { apiKey }
            )).FirstOrDefault();
    }
}