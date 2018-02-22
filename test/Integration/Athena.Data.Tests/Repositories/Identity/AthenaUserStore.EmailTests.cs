using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using AutoFixture.Xunit2;
using Xunit;

namespace Athena.Data.Tests.Repositories.Identity
{
    public partial class AthenaUserStoreTests
    {
        [Theory, AutoData]
        public async Task SetEmailValid(AthenaUser user, string newEmail)
        {
            await _sut.SetEmailAsync(user, newEmail, CancellationToken.None);
            
            Assert.Equal(user.Email, newEmail);
        }

        [Theory, AutoData]
        public async Task GetEmailValid(AthenaUser user)
        {
            var result = await _sut.GetEmailAsync(user, CancellationToken.None);
            
            Assert.Equal(user.Email, result);
        }

        [Theory, AutoData]
        public async Task GetEmailConfirmedValid(AthenaUser user)
        {
            var result = await _sut.GetEmailConfirmedAsync(user, CancellationToken.None);
            
            Assert.Equal(user.EmailConfirmed, result);
        }

        [Theory, AutoData]
        public async Task SetEmailConfirmedValid(AthenaUser user)
        {
            var expected = !user.EmailConfirmed;
            
            await _sut.SetEmailConfirmedAsync(user, expected, CancellationToken.None);
            
            Assert.Equal(expected, user.EmailConfirmed);
        }
        
        [Theory, AutoData]
        public async Task FindByEmailValid(AthenaUser user)
        {
            user.Student.Id = user.Id;
            
            await _students.AddAsync(user.Student);
            await _sut.CreateAsync(user, CancellationToken.None);

            var result = await _sut.FindByEmailAsync(user.NormalizedEmail, CancellationToken.None);
            
            Assert.Equal(user, result);
        }

        [Theory, AutoData]
        public async Task GetNormalizedEmailValid(AthenaUser user)
        {
            var result = await _sut.GetNormalizedEmailAsync(user, CancellationToken.None);
            
            Assert.Equal(user.NormalizedEmail, result);
        }

        [Theory, AutoData]
        public async Task SetNormalizedEmailAsync(AthenaUser user, string newEmail)
        {
            await _sut.SetNormalizedEmailAsync(user, newEmail, CancellationToken.None);
            
            Assert.Equal(newEmail, user.NormalizedEmail);
            Assert.Equal(newEmail, user.NormalizedEmail);
        }
    }
}