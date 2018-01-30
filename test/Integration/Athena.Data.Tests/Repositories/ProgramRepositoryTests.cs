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
    public class ProgramRepositoryTests : DataTest
    {
        private readonly InstitutionRepository _instutitons;
        private readonly ProgramRepository _sut;

        public ProgramRepositoryTests()
        {
            _instutitons = new InstitutionRepository(_db);
            _sut = new ProgramRepository(_db);
        }

        [Theory, AutoData]
        public async Task AddValid(Program program)
        {
            await _instutitons.AddAsync(program.Institution);
            await _sut.AddAsync(program);

            var result = await _sut.GetAsync(program.Id);
            
            Assert.Equal(program, result);
        }

        [Theory, AutoData]
        public async Task Add_ThrowsForDuplicate(Program program)
        {
            await _instutitons.AddAsync(program.Institution);
            await _sut.AddAsync(program);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () => await _sut.AddAsync(program));
        }

        [Theory, AutoData]
        public async Task EditValid(Program program, Program changes)
        {
            await _instutitons.AddAsync(program.Institution);
            await _instutitons.AddAsync(changes.Institution);

            await _sut.AddAsync(program);

            changes.Id = program.Id;
            await _sut.EditAsync(changes);

            var result = await _sut.GetAsync(changes.Id);
            Assert.Equal(changes, result);
        }

        [Theory, AutoData]
        public async Task DeleteValid(Program program)
        {
            await _instutitons.AddAsync(program.Institution);

            await _sut.AddAsync(program);
            Assert.NotNull(await _sut.GetAsync(program.Id));

            await _sut.DeleteAsync(program);
            Assert.Null(await _sut.GetAsync(program.Id));
        }

        [Theory, AutoData]
        public async Task CanGetAllProgramsForInstitution(List<Program> programs, Program exclude, Institution common)
        {
            await _instutitons.AddAsync(common);
            foreach (var p in programs)
            {
                p.Institution = common;
                await _sut.AddAsync(p);
            }
            
            await _instutitons.AddAsync(exclude.Institution);
            await _sut.AddAsync(exclude);

            var results = (await _sut.GetProgramsOfferedByInstitutionAsync(common)).ToList();
            
            Assert.Equal(programs.Count, results.Count);
            Assert.All(programs, p => Assert.Contains(p, results));
            Assert.DoesNotContain(exclude, results);
        }

        [Theory, AutoData]
        public async Task TracksRequirements(List<Requirement> requirements, Program program)
        {
            var requirementRepository = new RequirementRepository(_db);
            
            await _instutitons.AddAsync(program.Institution);
            await _sut.AddAsync(program);

            foreach (var r in requirements)
            {
                await requirementRepository.AddAsync(r);
                await _sut.AddRequirementAsync(program, r);
            }

            var results = (await requirementRepository.GetRequirementsForProgramAsync(program)).ToList();
            
            Assert.Equal(requirements.Count, results.Count);
            Assert.All(requirements, r => Assert.Contains(r, results));

            foreach (var r in requirements)
            {
                await _sut.RemoveRequirementAsync(program, r);
            }

            Assert.Empty(await requirementRepository.GetRequirementsForProgramAsync(program));
        }
    }
}