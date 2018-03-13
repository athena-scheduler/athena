using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Exceptions;
using System.Net;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/course/{id}/requirement")]
    public class RequirementCourseController : Controller
    {
        private readonly IRequirementRepository _requirements;
        private readonly ICourseRepository _courses;

        public RequirementCourseController(IRequirementRepository requirementsRepository, ICourseRepository coursesRepository)
        {
            _requirements = requirementsRepository ?? throw new ArgumentNullException(nameof(requirementsRepository));
            _courses = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
        }

        [HttpPost("prereq/{reqId}")]
        public async Task AddPrerequisiteAsync(Guid id, Guid reqId)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            var prereq = (await _requirements.GetAsync(reqId)).NotFoundIfNull();

            await _courses.AddPrerequisiteAsync(course, prereq);
        }

        [HttpDelete("prereq/{reqId}")]
        public async Task RemovePrerequisiteAsync(Guid id, Guid reqId)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            var prereq = (await _requirements.GetAsync(reqId)).NotFoundIfNull();

            await _courses.RemovePrerequisiteAsync(course, prereq);
        }

        [HttpPost("prereq/concurrent/{reqId}")]
        public async Task AddConcurrentPrerequisiteAsync(Guid id, Guid reqId)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            var prereq = (await _requirements.GetAsync(reqId)).NotFoundIfNull();
          
            await _courses.AddConcurrentPrerequisiteAsync(course, prereq);
        }

        [HttpDelete("prereq/concurrent/{reqId}")]
        public async Task RemoveConcurrentPrerequisiteAsync(Guid id, Guid reqId)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            var prereq = (await _requirements.GetAsync(reqId)).NotFoundIfNull();

            await _courses.RemoveConcurrentPrerequisiteAsync(course, prereq);
        }

        [HttpPost("satisfies/{reqId}")]
        public async Task AddSatisfiedRequirementAsync(Guid id, Guid reqId)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            var requirement = (await _requirements.GetAsync(reqId)).NotFoundIfNull();

            await _courses.AddSatisfiedRequirementAsync(course, requirement);
        }

        [HttpDelete("satisfies/{reqId}")]
        public async Task RemoveSatisfiedRequirementAsync(Guid id, Guid reqId)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            var requirement = (await _requirements.GetAsync(reqId)).NotFoundIfNull();

            await _courses.RemoveSatisfiedRequirementAsync(course, requirement);
        }
    }
}