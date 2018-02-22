using System;
using System.Threading;
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
                    Id = new Guid("f839e0f0-e29d-4c89-b406-51a52dbb08b5"),
                    Name = "Administrator"
                };

                role.NormalizedName = normalizer.Normalize(role.Name);

                if (roles.FindByIdAsync(role.Id.ToString(), CancellationToken.None).GetAwaiter().GetResult() != null)
                {
                    return;
                }

                if (roles.CreateAsync(role, CancellationToken.None).GetAwaiter().GetResult() != IdentityResult.Success)
                {
                    throw new MigrationException("Failed to create administrator role");
                }
            }
        }
    }
}
