using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Models.Identity;
using AutoFixture.Xunit2;
using Xunit;

namespace Athena.Data.Tests.Repositories.Identity
{
    public partial class AthenaUserStoreTests
    {
        [Theory, AutoData]
        public async Task AddToRoleAsync_Valid(AthenaUser user, AthenaRole role)
        {
            user.Id = user.Student.Id;
            await _students.AddAsync(user.Student);
            await _sut.CreateAsync(user, CancellationToken.None);
            await _roles.CreateAsync(role, CancellationToken.None);

            await _sut.AddToRoleAsync(user, role.NormalizedName, CancellationToken.None);

            var result = await _sut.GetUsersInRoleAsync(role.NormalizedName, CancellationToken.None);
            
            Assert.Equal(1, result.Count);
            Assert.Equal(user, result.FirstOrDefault());
        }
        
        [Theory, AutoData]
        public async Task RemoveFromRole_Valid(AthenaUser user, AthenaRole role)
        {
            user.Id = user.Student.Id;
            await _students.AddAsync(user.Student);
            await _sut.CreateAsync(user, CancellationToken.None);
            await _roles.CreateAsync(role, CancellationToken.None);

            await _sut.AddToRoleAsync(user, role.NormalizedName, CancellationToken.None);
            await _sut.RemoveFromRoleAsync(user, role.NormalizedName, CancellationToken.None);

            var result = await _sut.GetUsersInRoleAsync(role.NormalizedName, CancellationToken.None);
            
            Assert.Empty(result);
        }

        [Theory, AutoData]
        public async Task GetRolesValid(AthenaUser user, List<AthenaRole> roles)
        {
            user.Id = user.Student.Id;
            await _students.AddAsync(user.Student);
            await _sut.CreateAsync(user, CancellationToken.None);

            foreach (var role in roles)
            {
                await _roles.CreateAsync(role, CancellationToken.None);
                await _sut.AddToRoleAsync(user, role.NormalizedName, CancellationToken.None);
            }

            var result = await _sut.GetRolesAsync(user, CancellationToken.None);
            
            Assert.Equal(roles.Count, result.Count);
            Assert.All(roles, r => Assert.Contains(r.NormalizedName, result));
        }

        [Theory, AutoData]
        public async Task IsInRole_RolePresent(AthenaUser user, AthenaRole role)
        {
            user.Id = user.Student.Id;
            await _students.AddAsync(user.Student);
            await _sut.CreateAsync(user, CancellationToken.None);
            await _roles.CreateAsync(role, CancellationToken.None);

            await _sut.AddToRoleAsync(user, role.NormalizedName, CancellationToken.None);

            Assert.True(await _sut.IsInRoleAsync(user, role.NormalizedName, CancellationToken.None));
        }
        
        [Theory, AutoData]
        public async Task IsInRole_RoleAbsent(AthenaUser user, AthenaRole role)
        {
            user.Id = user.Student.Id;
            await _students.AddAsync(user.Student);
            await _sut.CreateAsync(user, CancellationToken.None);
            await _roles.CreateAsync(role, CancellationToken.None);

            Assert.False(await _sut.IsInRoleAsync(user, role.NormalizedName, CancellationToken.None));
        }
    }
}