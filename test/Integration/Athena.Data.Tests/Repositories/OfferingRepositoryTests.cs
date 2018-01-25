﻿using System.Threading.Tasks;
using Athena.Core.Exceptions;
using Athena.Core.Models;
using Athena.Data.Repositories;
using AutoFixture.Xunit2;
using Xunit;

namespace Athena.Data.Tests.Repositories
{
    public class OfferingRepositoryTests : DataTest
    {
        private readonly CampusRepository _campuses;
        private readonly OfferingRepository _sut;

        public OfferingRepositoryTests()
        {
            _campuses = new CampusRepository(_db);
            _sut = new OfferingRepository(_db);
        }

        [Theory, AutoData]
        public async Task AddValid(Offering offering)
        {
            await _campuses.AddAsync(offering.Campus);
            await _sut.AddAsync(offering);

            var result = await _sut.GetAsync(offering.Id);
            Assert.Equal(offering, result);
        }

        [Theory, AutoData]
        public async Task Add_ThrowsForDuplicate(Offering offering)
        {
            await _campuses.AddAsync(offering.Campus);
            await _sut.AddAsync(offering);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () => await _sut.AddAsync(offering));
        }

        [Theory, AutoData]
        public async Task EditValid(Offering offering, Offering changes)
        {
            await _campuses.AddAsync(offering.Campus);
            await _campuses.AddAsync(changes.Campus);
            await _sut.AddAsync(offering);

            changes.Id = offering.Id;
            await _sut.EditAsync(changes);

            var result = await _sut.GetAsync(changes.Id);
            Assert.Equal(changes, result);
        }

        [Theory, AutoData]
        public async Task DeleteValid(Offering offering)
        {
            await _campuses.AddAsync(offering.Campus);
            await _sut.AddAsync(offering);
            
            Assert.NotNull(await _sut.GetAsync(offering.Id));

            await _sut.DeleteAsync(offering);
            Assert.Null(await _sut.GetAsync(offering.Id));
        }
    }
}