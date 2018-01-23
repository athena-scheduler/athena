using System.Threading.Tasks;
using Athena.Core.Exceptions;
using Athena.Core.Models;
using Athena.Data.Repositories;
using AutoFixture.Xunit2;
using Xunit;

namespace Athena.Data.Tests.Repositories
{
    public class MeetingRepositoryTests : DataTest
    {
        private readonly MeetingRepository _sut;

        public MeetingRepositoryTests() => _sut = new MeetingRepository(_db);

        [Theory, AutoData]
        public async Task AddValid(Meeting meeting)
        {
            await _sut.AddAsync(meeting);

            var result = await _sut.GetAsync(meeting.Id);
            Assert.Equal(meeting, result);
        }

        [Theory, AutoData]
        public async Task Add_ThrowsForDuplicate(Meeting meeting)
        {
            await _sut.AddAsync(meeting);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () => await _sut.AddAsync(meeting));
        }

        [Theory, AutoData]
        public async Task EditValid(Meeting meeting, Meeting changes)
        {
            await _sut.AddAsync(meeting);

            changes.Id = meeting.Id;
            await _sut.EditAsync(changes);

            var result = await _sut.GetAsync(changes.Id);
            Assert.Equal(changes, result);
        }

        [Theory, AutoData]
        public async Task DeleteValid(Meeting meeting)
        {
            await _sut.AddAsync(meeting);
            Assert.NotNull(await _sut.GetAsync(meeting.Id));

            await _sut.DeleteAsync(meeting);
            Assert.Null(await _sut.GetAsync(meeting.Id));
        }
    }
}