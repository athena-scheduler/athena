using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Repositories;
using Athena.Extensions;
using Athena.Models;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers.api
{
    [Route("api/v1/student/{studentId}/schedule")]
    public class ScheduleController : AthenaApiController
    {
        private readonly IStudentRepository _students;
        private readonly IOfferingReository _offerings;
        private readonly ICourseRepository _courses;

        public ScheduleController(
            IStudentRepository students,
            IOfferingReository offerings,
            ICourseRepository courses
        )
        {
            _students = students ?? throw new ArgumentNullException(nameof(students));
            _offerings = offerings ?? throw new ArgumentNullException(nameof(offerings));
            _courses = courses ?? throw new ArgumentNullException(nameof(courses));
        }
        
        [HttpGet]
        public async Task<IEnumerable<ScheduleEntry>> GetCurrentSchedule(Guid studentId)
        {
            var student = (await _students.GetAsync(studentId)).NotFoundIfNull();
            var enrolledOfferings = await _offerings.GetInProgressOfferingsForStudentAsync(student);

            return enrolledOfferings.SelectMany(o => o.Meetings.Select(m => new ScheduleEntry(o, m)));
        }

        [HttpGet("offerings/available")]
        public async Task<IEnumerable<Offering>> FindOfferings(Guid studentId, string q)
        {
            var student = (await _students.GetAsync(studentId)).NotFoundIfNull();

            var results = new List<Offering>();
            var searchOptions = new CourseSearchOptions
            {
                StudentId = student.Id,
                Completed = false,
                IncludeInProgress = false,
                Query = q
            };
            
            foreach (var course in await _courses.SearchAsync(searchOptions))
            {
                results.AddRange(await _offerings.GetOfferingsForCourseAsync(course));
            }

            return results;
        }
    }
}