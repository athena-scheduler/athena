using System;
using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Models.Identity;
using Athena.Data.Repositories.Identity;
using Microsoft.AspNetCore.Identity;
using SimpleMigrations;
namespace Athena.Data.Migrations
{
    [Migration(20180215110432, "Identity")]
    public class Identity : ScriptMigration
    {
        public Identity() : base("20180215110432_Identity.sql")
        {
        }

        protected override void Up()
        {
            base.Up();
            
            using (var roles = new AthenaRoleStore(Connection))
            {
                var normalizer = new UpperInvariantLookupNormalizer();

                var role = new AthenaRole
                {
                    Id = AthenaRole.AdminRole,
                    Name = AthenaRole.AdminRoleName
                };

                role.NormalizedName = normalizer.Normalize(role.Name);

                if (Task.Run(() => roles.FindByIdAsync(role.Id.ToString(), CancellationToken.None)).GetAwaiter().GetResult() != null)
                {
                    return;
                }

                if (Task.Run(() => roles.CreateAsync(role, CancellationToken.None)).GetAwaiter().GetResult() != IdentityResult.Success)
                {
                    throw new MigrationException("Failed to create administrator role");
                }
            }
        }
    }
}
