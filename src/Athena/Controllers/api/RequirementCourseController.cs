using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/course/{id}/requirements")]
    public class RequirementCourseController : AthenaApiController
    {
        private readonly IRequirementRepository _requirements;
        private readonly ICourseRepository _courses;

        public RequirementCourseController(IRequirementRepository requirementsRepository, ICourseRepository coursesRepository)
        {
            _requirements = requirementsRepository ?? throw new ArgumentNullException(nameof(requirementsRepository));
            _courses = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
        }
        
        [HttpGet("prereq")]
        public async Task<IEnumerable<Requirement>> GetPrereqsForCourseAsync(Guid id)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            
            return await _requirements.GetPrereqsForCourseAsync(course);
        }

        [HttpPut("prereq/{reqId}")]
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
        
        [HttpGet("concurrent")]
        public async Task<IEnumerable<Requirement>> GetConcurrentPrereqsAsync(Guid id)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            
            return await _requirements.GetConcurrentPrereqsAsync(course);
        }

        [HttpPut("prereq/concurrent/{reqId}")]
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
        
        [HttpGet("satisfies")]
        public async Task<IEnumerable<Requirement>> GetRequirementsCourseSatisfiesAsync(Guid id)
        {
            var course =  (await _courses.GetAsync(id)).NotFoundIfNull();

            return await _requirements.GetRequirementsCourseSatisfiesAsync(course);
        }

        [HttpPut("satisfies/{reqId}")]
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