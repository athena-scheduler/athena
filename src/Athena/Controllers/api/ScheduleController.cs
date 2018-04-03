using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IInstitutionRepository _institutions;
        private readonly ICourseRepository _courses;

        public ScheduleController(
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
        
        [HttpGet]
        public async Task<IEnumerable<ScheduleEntry>> GetCurrentSchedule(Guid studentId)
        {
            var student = (await _students.GetAsync(studentId)).NotFoundIfNull();
            var enrolledOfferings = await _offerings.GetInProgressOfferingsForStudentAsync(student);

            return enrolledOfferings.SelectMany(o => o.Meetings.Select(m => new ScheduleEntry(o, m)));
        }
    }
}