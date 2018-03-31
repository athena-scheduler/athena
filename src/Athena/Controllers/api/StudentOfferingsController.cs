using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Repositories;
using Athena.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers.api
{
    [Route("api/v1/student/{id}/offerings")]
    public class StudentOfferingsController : AthenaApiController
    {
        private readonly IStudentRepository _students;
        private readonly IOfferingReository _offerings;
        private readonly IInstitutionRepository _institutions;
        private readonly ICourseRepository _courses;

        public StudentOfferingsController(
            IStudentRepository students,
            IOfferingReository offerings,
            IInstitutionRepository institutions,
            ICourseRepository courses
        )
        {
            _students = students ?? throw new ArgumentNullException(nameof(students));
            _offerings = offerings ?? throw new ArgumentNullException(nameof(offerings));
            _institutions = institutions ?? throw new ArgumentNullException(nameof(institutions));
            _courses = courses ?? throw new ArgumentNullException(nameof(courses));
        }

        [HttpGet("available")]
        public async Task<IEnumerable<Offering>> GetAvailableOfferings(Guid id)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();

            var currentlyEnrolledCourses =(await _offerings.GetInProgressOfferingsForStudentAsync(student))
                .Select(o => o.Course.Id)
                .ToHashSet();
            
            var candidateCourses = new List<Course>();

            foreach (var institution in await _institutions.GetInstitutionsForStudentAsync(student))
            {
                candidateCourses.AddRange(
                    (await _courses.GetCoursesForInstitutionAsync(institution))
                        .Where(c => !currentlyEnrolledCourses.Contains(c.Id))
                );
            }

            var results = new List<Offering>();

            foreach (var course in candidateCourses)
            {
                results.AddRange(await _offerings.GetOfferingsForCourseAsync(course));
            }

            return results;
        }

        [HttpGet]
        public async Task<IEnumerable<Offering>> GetEnrolledOfferings(Guid id)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();

            return await _offerings.GetInProgressOfferingsForStudentAsync(student);
        }

        [HttpPut("{offeringId}")]
        public async Task EnrollInOffering(Guid id, Guid offeringId)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            var offering = (await _offerings.GetAsync(offeringId)).NotFoundIfNull();

            await _offerings.EnrollStudentInOfferingAsync(student, offering);
        }

        [HttpDelete("{offeringId}")]
        public async Task UnenrollInOffering(Guid id, Guid offeringId)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            var offering = (await _offerings.GetAsync(offeringId)).NotFoundIfNull();

            await _offerings.UnenrollStudentInOfferingAsync(student, offering);
        }
    }
}