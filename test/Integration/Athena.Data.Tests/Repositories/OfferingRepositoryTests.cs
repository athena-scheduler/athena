using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Exceptions;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using Athena.Data.Repositories;
using Athena.Data.Repositories.Identity;
using AutoFixture.Xunit2;
using Xunit;

namespace Athena.Data.Tests.Repositories
{
    public class OfferingRepositoryTests : DataTest
    {
        private readonly InstitutionRepository _institutions;
        private readonly CampusRepository _campuses;
        private readonly CourseRepository _courses;
        private readonly OfferingRepository _sut;
        private readonly MeetingRepository _meetings;

        public OfferingRepositoryTests()
        {
            _campuses = new CampusRepository(_db);
            _courses = new CourseRepository(_db);
            _institutions = new InstitutionRepository(_db);
            _meetings = new MeetingRepository(_db);
            _sut = new OfferingRepository(_db, _meetings);
        }

        [Theory, AutoData]
        public async Task AddValid(Offering offering)
        {
            await _campuses.AddAsync(offering.Campus);
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            foreach (var m in offering.Meetings)
            {
                await _meetings.AddAsync(m);
            }
            await _sut.AddAsync(offering);

            var result = await _sut.GetAsync(offering.Id);
            Assert.Equal(offering, result);
        }

        [Theory, AutoData]
        public async Task Add_ThrowsForDuplicate(Offering offering)
        {
            await _campuses.AddAsync(offering.Campus);
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            foreach (var m in offering.Meetings)
            {
                await _meetings.AddAsync(m);
            }
            await _sut.AddAsync(offering);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () => await _sut.AddAsync(offering));
        }

        [Theory, AutoData]
        public async Task EditValid(Offering offering, Offering changes)
        {
            await _campuses.AddAsync(offering.Campus);
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            await _institutions.AddAsync(changes.Course.Institution);
            await _courses.AddAsync(changes.Course);
            await _campuses.AddAsync(changes.Campus);
            foreach (var m in offering.Meetings.Union(changes.Meetings))
            {
                await _meetings.AddAsync(m);
            }
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
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            foreach (var m in offering.Meetings)
            {
                await _meetings.AddAsync(m);
            }
            await _sut.AddAsync(offering);
            
            Assert.NotNull(await _sut.GetAsync(offering.Id));

            await _sut.DeleteAsync(offering);
            Assert.Null(await _sut.GetAsync(offering.Id));
        }

        [Theory, AutoData]
        public async Task TracksMeetings(List<Meeting> meetings, Offering offering)
        {
            offering.Meetings = Enumerable.Empty<Meeting>();
            await _campuses.AddAsync(offering.Campus);
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            await _sut.AddAsync(offering);
            foreach (var m in meetings)
            {
                await _meetings.AddAsync(m);
                await _sut.AddMeetingAsync(offering, m);
            }

            var results = (await _meetings.GetMeetingsForOfferingAsync(offering)).ToList();
            
            Assert.Equal(meetings.Count, results.Count);
            Assert.All(meetings, m => Assert.Contains(m, results));

            foreach (var m in meetings)
            {
                await _sut.RemoveMeetingAsync(offering, m);
            }
            
            Assert.Empty(await _meetings.GetMeetingsForOfferingAsync(offering));
        }

        [Theory, AutoData]
        public async Task Meeting_ThrowsForDuplicate(Meeting meeting, Offering offering)
        {
            offering.Meetings = Enumerable.Empty<Meeting>();
            
            await _campuses.AddAsync(offering.Campus);
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            await _sut.AddAsync(offering);
            await _meetings.AddAsync(meeting);

            await _sut.AddMeetingAsync(offering, meeting);
            await Assert.ThrowsAsync<DuplicateObjectException>(
                async () => await _sut.AddMeetingAsync(offering, meeting));
        }

        [Theory, AutoData]
        public async Task TracksInProgressOfferings(List<Offering> offerings, AthenaUser user)
        {
            var userRepo = new AthenaUserStore(_db);
            var studentRepo = new StudentRepository(_db);

            user.Student.Id = user.Id;
            await studentRepo.AddAsync(user.Student);
            await userRepo.CreateAsync(user, CancellationToken.None);

            foreach (var offering in offerings)
            {
                await _campuses.AddAsync(offering.Campus);
                await _institutions.AddAsync(offering.Course.Institution);
                await _courses.AddAsync(offering.Course);
                foreach (var m in offering.Meetings)
                {
                    await _meetings.AddAsync(m);
                }
                
                await _sut.AddAsync(offering);
                await _sut.EnrollStudentInOfferingAsync(user.Student, offering);
            }

            var results = (await _sut.GetInProgressOfferingsForStudentAsync(user.Student)).ToList();
            
            Assert.Equal(offerings.Count, results.Count);
            Assert.All(offerings, o => Assert.Contains(o, results));

            foreach (var offering in offerings)
            {
                await _sut.UnenrollStudentInOfferingAsync(user.Student, offering);
            }
            
            Assert.Empty(await _sut.GetInProgressOfferingsForStudentAsync(user.Student));
        }

        [Theory, AutoData]
        public async Task EnrollInOffering_ThrowsForDuplicate(Offering offering, AthenaUser user)
        {
            var userRepo = new AthenaUserStore(_db);
            var studentRepo = new StudentRepository(_db);

            user.Student.Id = user.Id;
            await studentRepo.AddAsync(user.Student);
            await userRepo.CreateAsync(user, CancellationToken.None);
            
            await _campuses.AddAsync(offering.Campus);
            await _institutions.AddAsync(offering.Course.Institution);
            await _courses.AddAsync(offering.Course);
            foreach (var m in offering.Meetings)
            {
                await _meetings.AddAsync(m);
            }
            await _sut.AddAsync(offering);

            await _sut.EnrollStudentInOfferingAsync(user.Student, offering);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () =>
                await _sut.EnrollStudentInOfferingAsync(user.Student, offering));
        }
    }
}