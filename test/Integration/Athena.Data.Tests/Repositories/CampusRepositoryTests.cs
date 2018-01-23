using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Exceptions;
using Athena.Core.Models;
using Athena.Data.Repositories;
using Athena.Data.Tests.Util;
using AutoFixture.Xunit2;
using Xunit;

namespace Athena.Data.Tests.Repositories
{
    public class CampusRepositoryTests : DataTest
    {
        private readonly IEqualityComparer<Campus> _comparator = new PropertyEqualityComparer<Campus>();
        private readonly CampusRepository _sut;

        public CampusRepositoryTests() => _sut = new CampusRepository(_db);

        [Theory, AutoData]
        public async Task AddValid(Campus campus)
        {
            await _sut.AddAsync(campus);

            var result = await _sut.GetAsync(campus.Id);
            Assert.Equal(campus, result, _comparator);
        }

        [Theory, AutoData]
        public async Task Add_ThrowsForDuplicate(Campus campus)
        {
            await _sut.AddAsync(campus);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () => await _sut.AddAsync(campus));
        }

        [Theory, AutoData]
        public async Task EditValid(Campus campus, Campus changes)
        {
            await _sut.AddAsync(campus);

            changes.Id = campus.Id;
            await _sut.EditAsync(changes);

            var result = await _sut.GetAsync(changes.Id);
            Assert.Equal(changes, result, _comparator);
        }

        [Theory, AutoData]
        public async Task DeleteValid(Campus campus)
        {
            await _sut.AddAsync(campus);
            Assert.NotNull(await _sut.GetAsync(campus.Id));

            await _sut.DeleteAsync(campus);
            Assert.Null(await _sut.GetAsync(campus.Id));
        }
    }
}