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
    public class StudentRepositoryTests : DataTest
    {
        private readonly StudentRepository _sut;

        public StudentRepositoryTests() => _sut = new StudentRepository(_db);

        [Theory, AutoData]
        public async Task AddValid(Student student)
        {
            await _sut.AddAsync(student);

            var result = await _sut.GetAsync(student.Id);
            
            Assert.Equal(student, result);
        }

        [Theory, AutoData]
        public async Task Add_ThrowsForDuplicate(Student student)
        {
            await _sut.AddAsync(student);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () => await _sut.AddAsync(student));
        }

        [Theory, AutoData]
        public async Task EditValid(Student student, Student changes)
        {
            await _sut.AddAsync(student);

            changes.Id = student.Id;
            await _sut.EditAsync(changes);

            var result = await _sut.GetAsync(changes.Id);
            Assert.Equal(changes, result);
        }

        [Theory, AutoData]
        public async Task DeleteValid(Student student)
        {
            await _sut.AddAsync(student);
            Assert.NotNull(await _sut.GetAsync(student.Id));

            await _sut.DeleteAsync(student);
            Assert.Null(await _sut.GetAsync(student.Id));
        }

        [Theory, AutoData]
        public async Task TracksPrograms(List<Program> programs, Student student, Institution common)
        {
            var institutionRepository = new InstitutionRepository(_db);
            var programRepository = new ProgramRepository(_db);
            
            await institutionRepository.AddAsync(common);
            await _sut.AddAsync(student);
            foreach (var p in programs)
            {
                p.Institution = common;
                await programRepository.AddAsync(p);
                await _sut.RegisterForProgramAsync(student, p);
            }

            var results = (await programRepository.GetProgramsForStudentAsync(student)).ToList();
            
            Assert.Equal(programs.Count, results.Count);
            Assert.All(programs, p => Assert.Contains(p, results));

            foreach (var p in programs)
            {
                await _sut.UnregisterForProgramAsync(student, p);
            }
            
            Assert.Empty(await programRepository.GetProgramsForStudentAsync(student));
        }

        [Theory, AutoData]
        public async Task Programs_ThrowsForDuplicate(Program program, Student student)
        {
            var institutionRepository = new InstitutionRepository(_db);
            var programRepository = new ProgramRepository(_db);
            
            await institutionRepository.AddAsync(program.Institution);
            await _sut.AddAsync(student);
            await programRepository.AddAsync(program);

            await _sut.RegisterForProgramAsync(student, program);
            await Assert.ThrowsAsync<DuplicateObjectException>(async () =>
                await _sut.RegisterForProgramAsync(student, program));
        }
    }
}