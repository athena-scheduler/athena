using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Exceptions;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Athena.Data.Tests.Repositories.Identity
{
    public partial class AthenaUserStoreTests
    {
        [Theory, AutoData]
        public async Task AddLoginValid(Student student, AthenaUser user, UserLoginInfo info)
        {
            student.Id = user.Id;
            await _students.AddAsync(student);
            await _sut.CreateAsync(user, CancellationToken.None);

            await _sut.AddLoginAsync(user, info, CancellationToken.None);

            var result = await _sut.FindByLoginAsync(info.LoginProvider, info.ProviderKey, CancellationToken.None);
            
            Assert.Equal(user, result);
        }

        [Theory, AutoData]
        public async Task AddLogin_ThrowsIfCancelled(AthenaUser user, UserLoginInfo info)
        {
            var ct = new CancellationTokenSource();
            ct.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _sut.AddLoginAsync(user, info, ct.Token));
        }

        [Theory, AutoData]
        public async Task AddLogin_ThrowsForDuplicate(Student student, AthenaUser user, UserLoginInfo info)
        {
            student.Id = user.Id;
            await _students.AddAsync(student);
            await _sut.CreateAsync(user, CancellationToken.None);

            await _sut.AddLoginAsync(user, info, CancellationToken.None);
            await Assert.ThrowsAsync<DuplicateObjectException>(async () =>
                await _sut.AddLoginAsync(user, info, CancellationToken.None));
        }

        [Theory, AutoData]
        public async Task RemoveLoginValid(Student student, AthenaUser user, UserLoginInfo info)
        {
            student.Id = user.Id;
            await _students.AddAsync(student);
            await _sut.CreateAsync(user, CancellationToken.None);

            await _sut.AddLoginAsync(user, info, CancellationToken.None);
            await _sut.RemoveLoginAsync(user, info.LoginProvider, info.ProviderKey, CancellationToken.None);
            
            Assert.Empty(await _sut.GetLoginsAsync(user, CancellationToken.None));
        }

        [Theory, AutoData]
        public async Task RemoveLogin_ThrowsIfCancelled(AthenaUser user, UserLoginInfo info)
        {
            var ct = new CancellationTokenSource();
            ct.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _sut.RemoveLoginAsync(user, info.LoginProvider, info.ProviderKey, ct.Token));
        }

        [Theory, AutoData]
        public async Task GetLoginsValid(Student student, AthenaUser user, List<UserLoginInfo> info)
        {
            student.Id = user.Id;
            await _students.AddAsync(student);
            await _sut.CreateAsync(user, CancellationToken.None);

            foreach (var i in info)
            {
                await _sut.AddLoginAsync(user, i, CancellationToken.None);
            }

            var result = await _sut.GetLoginsAsync(user, CancellationToken.None);
            
            Assert.Equal(info.Count, result.Count);
            Assert.All(info, i => Assert.Contains(result, r =>
                r.LoginProvider.Equals(i.LoginProvider) &&
                r.ProviderDisplayName.Equals(i.ProviderDisplayName) &&
                r.ProviderKey.Equals(i.ProviderKey)
            ));
        }
    }
}