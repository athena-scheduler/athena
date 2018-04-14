using System;
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
        public async Task TracksSecurityStamp(AthenaUser user, string stamp)
        {
            user.Id = user.Student.Id;
            await _students.AddAsync(user.Student);
            await _sut.CreateAsync(user, CancellationToken.None);

            await _sut.SetSecurityStampAsync(user, stamp, CancellationToken.None);
            await _sut.UpdateAsync(user, CancellationToken.None);

            var result = await _sut.FindByIdAsync(user.Id.ToString(), CancellationToken.None);
            
            Assert.Equal(user, result);
            Assert.Equal(stamp, await _sut.GetSecurityStampAsync(user, CancellationToken.None));
        }

        [Theory, AutoData]
        public async Task SetSecurityStamp_ThrowsIfCancelled(AthenaUser user, string stamp)
        {
            var ct = new CancellationTokenSource();
            ct.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _sut.SetSecurityStampAsync(user, stamp, ct.Token));
        }

        [Theory, AutoData]
        public async Task GetSecurityStamp_ThrowsIfCancelled(AthenaUser user)
        {
            var ct = new CancellationTokenSource();
            ct.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _sut.GetSecurityStampAsync(user, ct.Token));
        }
    }
}