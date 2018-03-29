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
    public class CourseRepositoryTests : DataTest
    {
        private readonly CourseRepository _sut;
        private readonly InstitutionRepository _institutions;

        public CourseRepositoryTests()
        {
            _sut = new CourseRepository(_db);
            _institutions = new InstitutionRepository(_db);
        }

        [Theory, AutoData]
        public async Task AddValid(Course course)
        {
            await _institutions.AddAsync(course.Institution);
            await _sut.AddAsync(course);

            var result = await _sut.GetAsync(course.Id);
            
            Assert.Equal(course, result);
        }

        [Theory, AutoData]
        public async Task Add_ThrowsForDuplicate(Course course)
        {
            await _institutions.AddAsync(course.Institution);
            await _sut.AddAsync(course);

            await Assert.ThrowsAsync<DuplicateObjectException>(async () => await _sut.AddAsync(course));
        }

        [Theory, AutoData]
        public async Task EditValid(Course course, Course changes)
        {
            await _institutions.AddAsync(course.Institution);
            await _institutions.AddAsync(changes.Institution);

            await _sut.AddAsync(course);

            changes.Id = course.Id;
            await _sut.EditAsync(changes);

            var result = await _sut.GetAsync(changes.Id);
            Assert.Equal(changes, result);            
        }

        [Theory, AutoData]
        public async Task DeleteValid(Course course)
        {
            await _institutions.AddAsync(course.Institution);
            await _sut.AddAsync(course);
            
            Assert.NotNull(await _sut.GetAsync(course.Id));

            await _sut.DeleteAsync(course);
            Assert.Null(await _sut.GetAsync(course.Id));
        }

        [Theory, AutoData]
        public async Task GetForInstitution(List<Course> courses, Institution common)
        {
            await _institutions.AddAsync(common);
            foreach (var course in courses)
            {
                course.Institution = common;
                await _sut.AddAsync(course);
            }

            var results = (await _sut.GetCoursesForInstitutionAsync(common)).ToList();
            
            Assert.Equal(courses.Count, results.Count);
            Assert.All(courses, c => Assert.Contains(c, results));
        }

        [Theory, AutoData]
        public async Task TracksRequirements(List<Requirement> requirements, Course course)
        {
            var requirementRepository = new RequirementRepository(_db);

            await _institutions.AddAsync(course.Institution);
            await _sut.AddAsync(course);

            foreach (var r in requirements)
            {
                await requirementRepository.AddAsync(r);
                await _sut.AddSatisfiedRequirementAsync(course, r);
            }

            var results = (await requirementRepository.GetRequirementsCourseSatisfiesAsync(course)).ToList();
            
            Assert.Equal(requirements.Count, results.Count);
            Assert.All(requirements, r => Assert.Contains(r, results));

            foreach (var r in requirements)
            {
                await _sut.RemoveSatisfiedRequirementAsync(course, r);
            }
            
            Assert.Empty(await requirementRepository.GetRequirementsCourseSatisfiesAsync(course));
        }

        [Theory, AutoData]
        public async Task Requirement_ThrowsForDuplicate(Requirement req, Course course)
        {
            var requirementRepository = new RequirementRepository(_db);

            await _institutions.AddAsync(course.Institution);
            await _sut.AddAsync(course);
            await requirementRepository.AddAsync(req);

            await _sut.AddSatisfiedRequirementAsync(course, req);
            await Assert.ThrowsAsync<DuplicateObjectException>(async () =>
                await _sut.AddSatisfiedRequirementAsync(course, req));
        }

        [Theory, AutoData]
        public async Task TracksPrereqs(List<Requirement> prereqs, Course course)
        {
            var requirementRepository = new RequirementRepository(_db);

            await _institutions.AddAsync(course.Institution);
            await _sut.AddAsync(course);

            foreach (var r in prereqs)
            {
                await requirementRepository.AddAsync(r);
                await _sut.AddPrerequisiteAsync(course, r);
            }

            var results = (await requirementRepository.GetPrereqsForCourseAsync(course)).ToList();
            
            Assert.Equal(prereqs.Count, results.Count);
            Assert.All(prereqs, r => Assert.Contains(r, results));

            foreach (var r in prereqs)
            {
                await _sut.RemovePrerequisiteAsync(course, r);
            }
            
            Assert.Empty(await requirementRepository.GetPrereqsForCourseAsync(course));
        }

        [Theory, AutoData]
        public async Task Prereq_ThrowsForDuplicate(Requirement prereq, Course course)
        {
            var requirementRepository = new RequirementRepository(_db);

            await _institutions.AddAsync(course.Institution);
            await _sut.AddAsync(course);
            await requirementRepository.AddAsync(prereq);

            await _sut.AddPrerequisiteAsync(course, prereq);
            await Assert.ThrowsAsync<DuplicateObjectException>(async () =>
                await _sut.AddPrerequisiteAsync(course, prereq));
        }
        
        [Theory, AutoData]
        public async Task TracksConcurrentPrereqs(List<Requirement> prereqs, Course course)
        {
            var requirementRepository = new RequirementRepository(_db);

            await _institutions.AddAsync(course.Institution);
            await _sut.AddAsync(course);

            foreach (var r in prereqs)
            {
                await requirementRepository.AddAsync(r);
                await _sut.AddConcurrentPrerequisiteAsync(course, r);
            }

            var results = (await requirementRepository.GetConcurrentPrereqsAsync(course)).ToList();
            
            Assert.Equal(prereqs.Count, results.Count);
            Assert.All(prereqs, r => Assert.Contains(r, results));

            foreach (var r in prereqs)
            {
                await _sut.RemoveConcurrentPrerequisiteAsync(course, r);
            }
            
            Assert.Empty(await requirementRepository.GetPrereqsForCourseAsync(course));
        }

        [Theory, AutoData]
        public async Task ConcurrentPrereq_ThrowsForDuplicate(Requirement prereq, Course course)
        {
            var requirementRepository = new RequirementRepository(_db);

            await _institutions.AddAsync(course.Institution);
            await _sut.AddAsync(course);
            await requirementRepository.AddAsync(prereq);

            await _sut.AddConcurrentPrerequisiteAsync(course, prereq);
            await Assert.ThrowsAsync<DuplicateObjectException>(async () =>
                await _sut.AddConcurrentPrerequisiteAsync(course, prereq));
        }

        [Theory, AutoData]
        public async Task TracksCompletedCoursesForStudent(List<Course> courses, Student student)
        {
            var studentRepo = new StudentRepository(_db);
            await studentRepo.AddAsync(student);

            foreach (var c in courses)
            {
                await _institutions.AddAsync(c.Institution);
                await _sut.AddAsync(c);
                await _sut.MarkCourseAsCompletedForStudentAsync(c, student);
            }

            var results = (await _sut.GetCompletedCoursesForStudentAsync(student)).ToList();
            
            Assert.Equal(courses.Count, results.Count);
            Assert.All(courses, c => Assert.Contains(c, results));

            foreach (var c in courses)
            {
                await _sut.MarkCourseAsUncompletedForStudentAsync(c, student);
            }
            
            Assert.Empty(await _sut.GetCompletedCoursesForStudentAsync(student));
        }

        [Theory, AutoData]
        public async Task CompletedCourses_ThrowsForDuplicate(Course course, Student student)
        {
            var studentRepo = new StudentRepository(_db);
            await studentRepo.AddAsync(student);
            await _institutions.AddAsync(course.Institution);
            await _sut.AddAsync(course);
            
            await _sut.MarkCourseAsCompletedForStudentAsync(course, student);
            await Assert.ThrowsAsync<DuplicateObjectException>(async () =>
                await _sut.MarkCourseAsCompletedForStudentAsync(course, student));
        }

        [Theory, AutoData]
        public async Task TracksOfferings(List<Offering> offerings, Course common)
        {
            var campusRepo = new CampusRepository(_db);
            var meetingRepo = new MeetingRepository(_db);
            var offeringRepo = new OfferingRepository(_db, meetingRepo);

            await _institutions.AddAsync(common.Institution);
            await _sut.AddAsync(common);

            foreach (var o in offerings)
            {
                o.Course = common;
                await campusRepo.AddAsync(o.Campus);

                foreach (var m in o.Meetings)
                {
                    await meetingRepo.AddAsync(m);
                }

                await offeringRepo.AddAsync(o);
            }

            var results = (await offeringRepo.GetOfferingsForCourseAsync(common)).ToList();
            
            Assert.Equal(offerings.Count, results.Count);
            Assert.All(offerings, o => Assert.Contains(o, results));
        }
    }
}