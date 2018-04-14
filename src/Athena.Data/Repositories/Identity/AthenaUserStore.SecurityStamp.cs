using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Athena.Data.Repositories.Identity
{
    public partial class AthenaUserStore : IUserSecurityStampStore<AthenaUser>
    {
        public Task SetSecurityStampAsync(AthenaUser user, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.SecurityStamp = stamp;

            return Task.CompletedTask;
        }

        public Task<string> GetSecurityStampAsync(AthenaUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.SecurityStamp);
        }
    }
}