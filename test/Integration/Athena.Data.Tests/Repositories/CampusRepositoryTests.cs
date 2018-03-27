using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athena.Core.Exceptions;
using Athena.Core.Models;
using Athena.Data.Repositories;
using AutoFixture.Xunit2;
using Xunit;

namespace Athena.Data.Tests.Repositories
{
    public class CampusRepositoryTests : DataTest
    {
        private readonly CampusRepository _sut;

        public CampusRepositoryTests() => _sut = new CampusRepository(_db);

        [Theory, AutoData]
        public async Task AddValid(Campus campus)
        {
            await _sut.AddAsync(campus);

            var result = await _sut.GetAsync(campus.Id);
            Assert.Equal(campus, result);
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
            Assert.Equal(changes, result);
        }

        [Theory, AutoData]
        public async Task DeleteValid(Campus campus)
        {
            await _sut.AddAsync(campus);
            Assert.NotNull(await _sut.GetAsync(campus.Id));

            await _sut.DeleteAsync(campus);
            Assert.Null(await _sut.GetAsync(campus.Id));
        }

        [Theory, AutoData]
        public async Task CanAssociateCampusWithInstitution(List<Campus> campuses, Institution institution)
        {
            var institutionRepo = new InstitutionRepository(_db);

            await institutionRepo.AddAsync(institution);
            foreach (var campus in campuses)
            {
                await _sut.AddAsync(campus);
                await _sut.AssociateCampusWithInstitutionAsync(campus, institution);
            }

            var results = (await _sut.GetCampusesForInstitutionAsync(institution)).ToList();
            
            Assert.Equal(campuses.Count, results.Count);
            Assert.All(campuses, c => Assert.Contains(c, results));
        }

        [Theory, AutoData]
        public async Task AssociateCampusWithInstitution_ThrowsForDuplicate(Campus campus, Institution institution)
        {
            var institutionRepo = new InstitutionRepository(_db);

            await institutionRepo.AddAsync(institution);
            await _sut.AddAsync(campus);

            await _sut.AssociateCampusWithInstitutionAsync(campus, institution);

            await Assert.ThrowsAnyAsync<DuplicateObjectException>(async () =>
                await _sut.AssociateCampusWithInstitutionAsync(campus, institution));
        }
        
        [Theory, AutoData]
        public async Task CanDisassociateCampusWithInstitution(Campus extra, List<Campus> campuses, Institution institution)
        {
            var institutionRepo = new InstitutionRepository(_db);

            await institutionRepo.AddAsync(institution);
            foreach (var campus in campuses.Union(new[] {extra}))
            {
                await _sut.AddAsync(campus);
                await _sut.AssociateCampusWithInstitutionAsync(campus, institution);
            }

            var results = (await _sut.GetCampusesForInstitutionAsync(institution)).ToList();
            
            Assert.Equal(campuses.Count + 1, results.Count);
            Assert.All(campuses, c => Assert.Contains(c, results));
            Assert.Contains(extra, results);

            await _sut.DissassociateCampusWithInstitutionAsync(extra, institution);
            
            results = (await _sut.GetCampusesForInstitutionAsync(institution)).ToList();
            
            Assert.Equal(campuses.Count, results.Count);
            Assert.All(campuses, c => Assert.Contains(c, results));
            Assert.DoesNotContain(extra, results);
        }
    }
}