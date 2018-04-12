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
    public class RequirementRepositoryTests : DataTest
    {
        private readonly RequirementRepository _sut;

        public RequirementRepositoryTests() => _sut = new RequirementRepository(_db);

        [Theory, AutoData]
        public async Task AddValid(Requirement requirement)
        {
            await _sut.AddAsync(requirement);

            var result = await _sut.GetAsync(requirement.Id);
            
            Assert.Equal(requirement, result);
        }

        [Theory, AutoData]
        public async Task Add_ThrowsForDuplicate(Requirement requirement)
        {
            await _sut.AddAsync(requirement);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () => await _sut.AddAsync(requirement));
        }

        [Theory, AutoData]
        public async Task EditValid(Requirement requirement, Requirement changes)
        {
            await _sut.AddAsync(requirement);

            changes.Id = requirement.Id;
            await _sut.EditAsync(changes);

            var result = await _sut.GetAsync(changes.Id);
            Assert.Equal(changes, result);
        }

        [Theory, AutoData]
        public async Task DeleteValid(Requirement requirement)
        {
            await _sut.AddAsync(requirement);
            Assert.NotNull(await _sut.GetAsync(requirement.Id));

            await _sut.DeleteAsync(requirement);
            Assert.Null(await _sut.GetAsync(requirement.Id));
        }

        [Theory, AutoData]
        public async Task GetInProgressRequirementsValid(
            Dictionary<Offering, List<Requirement>> testData,
            KeyValuePair<Offering, List<Requirement>> exclude,
            AthenaUser user)
        {
            var campuses = new CampusRepository(_db);
            var courses = new CourseRepository(_db, _sut);
            var institutions = new InstitutionRepository(_db);
            var meetings = new MeetingRepository(_db);
            var offerings = new OfferingRepository(_db, meetings);
            var userRepo = new AthenaUserStore(_db);
            var studentRepo = new StudentRepository(_db);

            user.Student.Id = user.Id;
            await studentRepo.AddAsync(user.Student);
            await userRepo.CreateAsync(user, CancellationToken.None);


            foreach (var (offering, reqs) in testData.Union(new [] { exclude }))
            {
                await campuses.AddAsync(offering.Campus);
                await institutions.AddAsync(offering.Course.Institution);
                await courses.AddAsync(offering.Course);
                await offerings.AddAsync(offering);
                foreach (var meeting in offering.Meetings)
                {
                    meeting.Offering = offering.Id;
                    await meetings.AddAsync(meeting);
                }

                foreach (var req in reqs)
                {
                    await _sut.AddAsync(req);
                    await courses.AddSatisfiedRequirementAsync(offering.Course, req);
                }
            }

            foreach (var offering in testData.Keys)
            {
                await offerings.EnrollStudentInOfferingAsync(user.Student, offering);
            }

            var expected = testData.Values.SelectMany(r => r).ToList();
            var results = (await _sut.GetInProgressRequirementsForStudentAsync(user.Student)).ToList();
            
            Assert.Equal(expected.Count, results.Count);
            Assert.All(expected, r => Assert.Contains(r, results));
        }
        
        [Theory, AutoData]
        public async Task GetCompletedRequirementsValid(
            Dictionary<Offering, List<Requirement>> testData,
            KeyValuePair<Offering, List<Requirement>> exclude,
            AthenaUser user)
        {
            var campuses = new CampusRepository(_db);
            var courses = new CourseRepository(_db, _sut);
            var institutions = new InstitutionRepository(_db);
            var meetings = new MeetingRepository(_db);
            var offerings = new OfferingRepository(_db, meetings);
            var userRepo = new AthenaUserStore(_db);
            var studentRepo = new StudentRepository(_db);

            user.Student.Id = user.Id;
            await studentRepo.AddAsync(user.Student);
            await userRepo.CreateAsync(user, CancellationToken.None);
            
            foreach (var (offering, reqs) in testData.Union(new [] { exclude }))
            {
                await campuses.AddAsync(offering.Campus);
                await institutions.AddAsync(offering.Course.Institution);
                await courses.AddAsync(offering.Course);
                await offerings.AddAsync(offering);
                foreach (var meeting in offering.Meetings)
                {
                    meeting.Offering = offering.Id;
                    await meetings.AddAsync(meeting);
                }

                foreach (var req in reqs)
                {
                    await _sut.AddAsync(req);
                    await courses.AddSatisfiedRequirementAsync(offering.Course, req);
                }
            }

            foreach (var offering in testData.Keys)
            {
                await courses.MarkCourseAsCompletedForStudentAsync(offering.Course, user.Student);
            }

            var expected = testData.Values.SelectMany(r => r).ToList();
            var results = (await _sut.GetCompletedRequirementsForStudentAsync(user.Student)).ToList();
            
            Assert.Equal(expected.Count, results.Count);
            Assert.All(expected, r => Assert.Contains(r, results));
        }
    }
}