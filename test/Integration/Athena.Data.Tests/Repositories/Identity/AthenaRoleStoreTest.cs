using System;
using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Models.Identity;
using Athena.Data.Repositories.Identity;
using AutoFixture.Xunit2;
using Xunit;

namespace Athena.Data.Tests.Repositories.Identity
{
    public class AthenaRoleStoreTest : DataTest
    {
        private readonly AthenaRoleStore _sut;

        public AthenaRoleStoreTest() => _sut = new AthenaRoleStore(_db);

        [Theory, AutoData]
        public async Task CreateValid(AthenaRole role)
        {
            await _sut.CreateAsync(role, CancellationToken.None);

            var result = await _sut.FindByIdAsync(role.Id.ToString(), CancellationToken.None);
            
            Assert.Equal(role, result);
        }

        [Theory, AutoData]
        public async Task Create_ThrowsIfCancelled(AthenaRole role)
        {
            var ct = new CancellationTokenSource();
            ct.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _sut.CreateAsync(role, ct.Token));
        }

        [Theory, AutoData]
        public async Task Create_AddsGuidIfEmpty(AthenaRole role)
        {
            role.Id = Guid.Empty;

            await _sut.CreateAsync(role, CancellationToken.None);

            var result = await _sut.FindByNameAsync(role.NormalizedName, CancellationToken.None);
            
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        [Theory, AutoData]
        public async Task Create_FailureForNonUnique(AthenaRole role)
        {
            var result = await _sut.CreateAsync(role, CancellationToken.None);
            Assert.True(result.Succeeded);

            result = await _sut.CreateAsync(role, CancellationToken.None);
            Assert.False(result.Succeeded);
        }

        [Theory, AutoData]
        public async Task UpdateValid(AthenaRole role, AthenaRole changes)
        {
            await _sut.CreateAsync(role, CancellationToken.None);

            changes.Id = role.Id;
            await _sut.UpdateAsync(changes, CancellationToken.None);

            var result = await _sut.FindByIdAsync(changes.Id.ToString(), CancellationToken.None);
            Assert.Equal(changes, result);
        }

        [Theory, AutoData]
        public async Task Update_ThrowsIfCancelled(AthenaRole role, AthenaRole changes)
        {
            await _sut.CreateAsync(role, CancellationToken.None);

            changes.Id = role.Id;
            var ct = new CancellationTokenSource();
            ct.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _sut.UpdateAsync(changes, ct.Token));
        }

        [Theory, AutoData]
        public async Task DeleteValid(AthenaRole role)
        {
            await _sut.CreateAsync(role, CancellationToken.None);

            await _sut.DeleteAsync(role, CancellationToken.None);
            
            Assert.Null(await _sut.FindByIdAsync(role.Id.ToString(), CancellationToken.None));
        }

        [Theory, AutoData]
        public async Task Delete_ThrowsIfCancelled(AthenaRole role)
        {
            await _sut.CreateAsync(role, CancellationToken.None);

            var ct = new CancellationTokenSource();
            ct.Cancel();
            
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _sut.DeleteAsync(role, ct.Token));
        }

        [Theory, AutoData]
        public async Task GetRoleIdValid(AthenaRole role)
        {
            var result = await _sut.GetRoleIdAsync(role, CancellationToken.None);
            
            Assert.Equal(role.Id.ToString(), result);
        }

        [Theory, AutoData]
        public async Task GetRoleNameAsync(AthenaRole role)
        {
            var result = await _sut.GetRoleNameAsync(role, CancellationToken.None);
            
            Assert.Equal(role.Name, result);
        }

        [Theory, AutoData]
        public async Task SetRoleNameValid(AthenaRole role, string newName)
        {
            await _sut.SetRoleNameAsync(role, newName, CancellationToken.None);
            
            Assert.Equal(newName, role.Name);
        }

        [Theory, AutoData]
        public async Task GetNormalizedRoleNameValid(AthenaRole role)
        {
            var result = await _sut.GetNormalizedRoleNameAsync(role, CancellationToken.None);
            
            Assert.Equal(role.NormalizedName, result);
        }

        [Theory, AutoData]
        public async Task SetNormalizedRoleNameValid(AthenaRole role, string newName)
        {
            await _sut.SetNormalizedRoleNameAsync(role, newName, CancellationToken.None);
            
            Assert.Equal(newName, role.NormalizedName);
        }
    }
}