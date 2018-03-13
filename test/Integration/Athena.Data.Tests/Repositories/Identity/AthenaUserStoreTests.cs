using System;
using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using Athena.Data.Repositories;
using Athena.Data.Repositories.Identity;
using AutoFixture.Xunit2;
using Xunit;

namespace Athena.Data.Tests.Repositories.Identity
{
    public partial class AthenaUserStoreTests : DataTest
    {
        private readonly StudentRepository _students;
        private readonly AthenaUserStore _sut;
        private readonly AthenaRoleStore _roles;

        public AthenaUserStoreTests()
        {
            _students = new StudentRepository(_db);
            _sut = new AthenaUserStore(_db);
            _roles = new AthenaRoleStore(_db);
        }

        [Theory, AutoData]
        public async Task GetUserIdValid(AthenaUser user)
        {
            var result = await _sut.GetUserIdAsync(user, CancellationToken.None);
            
            Assert.Equal(user.Id.ToString(), result);
        }

        [Theory, AutoData]
        public async Task GetUserNameValid(AthenaUser user)
        {
            var result = await _sut.GetUserNameAsync(user, CancellationToken.None);
            
            Assert.Equal(user.UserName, result);
        }

        [Theory, AutoData]
        public async Task SetUserNameValid(AthenaUser user, string newName)
        {
            await _sut.SetUserNameAsync(user, newName, CancellationToken.None);
            
            Assert.Equal(newName, user.UserName);
        }

        [Theory, AutoData]
        public async Task GetNormalizedUserNameValid(AthenaUser user)
        {
            var result = await _sut.GetNormalizedUserNameAsync(user, CancellationToken.None);
            
            Assert.Equal(user.NormalizedUserName, result);
        }

        [Theory, AutoData]
        public async Task SetNormalizedUserNameValid(AthenaUser user, string newName)
        {
            await _sut.SetNormalizedUserNameAsync(user, newName, CancellationToken.None);
            
            Assert.Equal(newName, user.NormalizedUserName);
        }

        [Theory, AutoData]
        public async Task CreateValid(AthenaUser user)
        {
            user.Student.Id = user.Id;
            await _students.AddAsync(user.Student);
            
            await _sut.CreateAsync(user, CancellationToken.None);

            var result = await _sut.FindByIdAsync(user.Id.ToString(), CancellationToken.None);
            
            Assert.Equal(user, result);
        }

        [Theory, AutoData]
        public async Task Create_ThrowsIfCancelled(AthenaUser user)
        {
            var ct = new CancellationTokenSource();
            ct.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _sut.CreateAsync(user, ct.Token));
        }

        [Theory, AutoData]
        public async Task Create_FailsForEmptyGuid(AthenaUser user)
        {
            user.Id = Guid.Empty;

            var result = await _sut.CreateAsync(user, CancellationToken.None);

            Assert.False(result.Succeeded);
        }

        [Theory, AutoData]
        public async Task Create_FailureForDuplicate(AthenaUser user)
        {
            user.Student.Id = user.Id;
            await _students.AddAsync(user.Student);
            
            var result = await _sut.CreateAsync(user, CancellationToken.None);
            Assert.True(result.Succeeded);

            result = await _sut.CreateAsync(user, CancellationToken.None);
            Assert.False(result.Succeeded);
        }

        [Theory, AutoData]
        public async Task UpdateValid(AthenaUser user, AthenaUser changes)
        {
            user.Student.Id = user.Id;
            await _students.AddAsync(user.Student);
            await _sut.CreateAsync(user, CancellationToken.None);

            changes.Id = user.Id;
            changes.Student = user.Student;
            await _sut.UpdateAsync(changes, CancellationToken.None);

            var result = await _sut.FindByIdAsync(changes.Id.ToString(), CancellationToken.None);
            
            Assert.Equal(changes, result);
        }

        [Theory, AutoData]
        public async Task Update_ThrowsIfCancelled(AthenaUser user, AthenaUser changes)
        {
            await _sut.CreateAsync(user, CancellationToken.None);

            changes.Id = user.Id;
            var ct = new CancellationTokenSource();
            ct.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _sut.UpdateAsync(changes, ct.Token));
        }

        [Theory, AutoData]
        public async Task DeleteValid(AthenaUser user)
        {
            user.Student.Id = user.Id;
            await _students.AddAsync(user.Student);
            await _sut.CreateAsync(user, CancellationToken.None);

            await _sut.DeleteAsync(user, CancellationToken.None);
            
            Assert.Null(await _sut.FindByIdAsync(user.Id.ToString(), CancellationToken.None));
        }

        [Theory, AutoData]
        public async Task Delete_ThrowsIfCancelled(AthenaUser user)
        {
            await _sut.CreateAsync(user, CancellationToken.None);

            var ct = new CancellationTokenSource();
            ct.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _sut.DeleteAsync(user, ct.Token));
        }
    }
}