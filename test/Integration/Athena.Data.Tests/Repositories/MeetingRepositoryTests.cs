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
        private readonly InstitutionRepository _institutions;
        private readonly CampusRepository _campuses;
        private readonly CourseRepository _courses;
        private readonly OfferingRepository _offerings;
        private readonly MeetingRepository _sut;

        public MeetingRepositoryTests()
        {
            _institutions = new InstitutionRepository(_db);
            _campuses = new CampusRepository(_db);
            _courses = new CourseRepository(_db);
            _sut = new MeetingRepository(_db);
            _offerings = new OfferingRepository(_db, _sut);
        }

        [Theory, AutoData]
        public async Task AddValid(Offering offering, Meeting meeting)
        {
            await _campuses.AddAsync(offering.Campus);
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            await _offerings.AddAsync(offering);
            meeting.Offering = offering.Id;

            await _sut.AddAsync(meeting);

            var result = await _sut.GetAsync(meeting.Id);
            Assert.Equal(meeting, result);
        }

        [Theory, AutoData]
        public async Task Add_ThrowsForDuplicate(Offering offering, Meeting meeting)
        {
            await _campuses.AddAsync(offering.Campus);
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            await _offerings.AddAsync(offering);
            meeting.Offering = offering.Id;
            
            await _sut.AddAsync(meeting);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () => await _sut.AddAsync(meeting));
        }

        [Theory, AutoData]
        public async Task EditValid(Offering offering, Meeting meeting, Meeting changes)
        {
            await _campuses.AddAsync(offering.Campus);
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            await _offerings.AddAsync(offering);
            changes.Offering = meeting.Offering = offering.Id;
            
            await _sut.AddAsync(meeting);

            changes.Id = meeting.Id;
            await _sut.EditAsync(changes);

            var result = await _sut.GetAsync(changes.Id);
            Assert.Equal(changes, result);
        }

        [Theory, AutoData]
        public async Task DeleteValid(Offering offering, Meeting meeting)
        {
            await _campuses.AddAsync(offering.Campus);
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            await _offerings.AddAsync(offering);
            meeting.Offering = offering.Id;
            
            await _sut.AddAsync(meeting);
            Assert.NotNull(await _sut.GetAsync(meeting.Id));

            await _sut.DeleteAsync(meeting);
            Assert.Null(await _sut.GetAsync(meeting.Id));
        }

        [Theory, AutoData]
        public async Task GetForOfferings_Valid(Offering offering, List<Meeting> meetings)
        {
            await _campuses.AddAsync(offering.Campus);
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            await _offerings.AddAsync(offering);

            foreach (var m in meetings)
            {
                m.Offering = offering.Id;
                await _sut.AddAsync(m);
            }

            var results = (await _sut.GetMeetingsForOfferingAsync(offering)).ToList();
            
            Assert.Equal(results.Count, meetings.Count);
            Assert.All(meetings, m => Assert.Contains(m, results));
        }
    }
}