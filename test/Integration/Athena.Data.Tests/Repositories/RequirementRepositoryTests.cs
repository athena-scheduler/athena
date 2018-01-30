using System.Threading.Tasks;
using Athena.Core.Exceptions;
using Athena.Core.Models;
using Athena.Data.Repositories;
using AutoFixture.Xunit2;
using Xunit;

namespace Athena.Data.Tests.Repositories
{
    public class RequirementRepositoryTests : DataTest
    {
        private readonly RequirementRepository _sut;

        public RequirementRepositoryTests() => _sut = new RequirementRepository(_db);

        [Theory, AutoData]
        public async Task AddValid(Requirement requirement)
        {
            await _sut.AddAsync(requirement);

            var result = await _sut.GetAsync(requirement.Id);
            
            Assert.Equal(requirement, result);
        }

        [Theory, AutoData]
        public async Task Add_ThrowsForDuplicate(Requirement requirement)
        {
            await _sut.AddAsync(requirement);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () => await _sut.AddAsync(requirement));
        }

        [Theory, AutoData]
        public async Task EditValid(Requirement requirement, Requirement changes)
        {
            await _sut.AddAsync(requirement);

            changes.Id = requirement.Id;
            await _sut.EditAsync(changes);

            var result = await _sut.GetAsync(changes.Id);
            Assert.Equal(changes, result);
        }

        [Theory, AutoData]
        public async Task DeleteValid(Requirement requirement)
        {
            await _sut.AddAsync(requirement);
            Assert.NotNull(await _sut.GetAsync(requirement.Id));

            await _sut.DeleteAsync(requirement);
            Assert.Null(await _sut.GetAsync(requirement.Id));
        }
    }
}