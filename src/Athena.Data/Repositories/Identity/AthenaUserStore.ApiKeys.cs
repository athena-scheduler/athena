using System.Linq;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using Dapper;

namespace Athena.Data.Repositories.Identity
{
    public partial class AthenaUserStore : IUserApiKeyStore
    {
        public async Task<AthenaUser> GetUserForApiKey(string apiKey) =>
            (await _db.QueryAsync<AthenaUser, Student, AthenaUser>($@"
                SELECT {UserProps}
                FROM users u
                    LEFT JOIN students s ON u.id = s.id
                WHERE u.api_key = @apiKey",
                MapUser,
                new { apiKey }
            )).FirstOrDefault();
    }
}