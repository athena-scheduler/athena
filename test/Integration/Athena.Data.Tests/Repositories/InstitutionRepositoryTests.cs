using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athena.Core.Exceptions;
using Athena.Core.Models;
using Athena.Data.Repositories;
using Athena.Data.Tests.Util;
using AutoFixture.Xunit2;
using Xunit;

namespace Athena.Data.Tests.Repositories
{
    public class InstitutionRepositoryTests : DataTest
    {
        private readonly IEqualityComparer<Institution> _comparator = new PropertyEqualityComparer<Institution>();
        private readonly InstitutionRepository _sut;

        public InstitutionRepositoryTests() => _sut = new InstitutionRepository(_db);

        [Theory, AutoData]
        public async Task AddValid(Institution institution)
        {
            await _sut.AddAsync(institution);

            var result = await _sut.GetAsync(institution.Id);
            Assert.Equal(institution, result, _comparator);
        }

        [Theory, AutoData]
        public async Task Add_ThrowsForDuplicate(Institution institution)
        {
            await _sut.AddAsync(institution);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () => await _sut.AddAsync(institution));
        }

        [Theory, AutoData]
        public async Task EditValid(Institution institution, Institution changes)
        {
            await _sut.AddAsync(institution);

            changes.Id = institution.Id;
            await _sut.EditAsync(changes);

            var result = await _sut.GetAsync(changes.Id);
            Assert.Equal(changes, result, _comparator);
        }

        [Theory, AutoData]
        public async Task DeleteValid(Institution institution)
        {
            await _sut.AddAsync(institution);
            Assert.NotNull(await _sut.GetAsync(institution.Id));

            await _sut.DeleteAsync(institution);
            Assert.Null(await _sut.GetAsync(institution.Id));
        }

        [Theory, AutoData]
        public async Task CanGetInstitutionsOnCampus(Campus campus, List<Institution> institutions)
        {
            var campusRepo = new CampusRepository(_db);

            await campusRepo.AddAsync(campus);
            foreach (var institution in institutions)
            {
                await _sut.AddAsync(institution);
                await campusRepo.AssociateCampusWithInstitutionAsync(campus, institution);
            }

            var results = (await _sut.GetInstitutionsOnCampusAsync(campus)).ToList();
            
            Assert.Equal(institutions.Count, results.Count);
            Assert.All(institutions, i => Assert.Contains(i, results, _comparator));
        }
    }
}