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

        [Theory, AutoData]
        public async Task GetMeetingsForOffering(List<Meeting> meetings, Meeting extra, Offering offering)
        {
            var _c = new CampusRepository(_db);
            var _o = new OfferingRepository(_db);

            await _c.AddAsync(offering.Campus);
            await _o.AddAsync(offering);

            foreach (var meeting in meetings.Union(new [] {extra}))
            {
                await _sut.AddAsync(meeting);
                await _o.AddMeetingAsync(offering, meeting);
            }

            var results = (await _sut.GetMeetingsForOfferingAsync(offering)).ToList();
            
            Assert.Equal(meetings.Count + 1, results.Count);
            Assert.All(meetings, m => Assert.Contains(m, results));
            Assert.Contains(extra, results);

            await _o.RemoveMeetingAsync(offering, extra);
            results = (await _sut.GetMeetingsForOfferingAsync(offering)).ToList();
            
            Assert.Equal(meetings.Count, results.Count);
            Assert.All(meetings, m => Assert.Contains(m, results));
            Assert.DoesNotContain(extra, results);
        }
    }
}