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
        public async Task AddPrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to add prereq {prereq} to course {course} that doesn't exist");
            }
            await _courses.AddPrerequisiteAsync(course, prereq);
        }

        [HttpDelete("prereq/{reqId}")]
        public async Task RemovePrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to remove prereq {prereq} to course {course} that doesn't exist");
            }
            await _courses.RemovePrerequisiteAsync(course, prereq);
        }

        [HttpPost("prereq/concurrent/{reqId}")]
        public async Task AddConcurrentPrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to add concurrent prereq {prereq} to course {course} that doesn't exist");
            }
            await _courses.AddConcurrentPrerequisiteAsync(course, prereq);
        }

        [HttpDelete("prereq/concurrent/{reqId}")]
        public async Task RemoveConcurrentPrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to remove concurrent prereq {prereq} to course {course} that doesn't exist");
            }
            await _courses.RemoveConcurrentPrerequisiteAsync(course, prereq);
        }

        [HttpPost("satisfies/{reqId}")]
        public async Task AddSatisfiedRequirementAsync(Course course, Requirement requirement)
        {
            if (requirement == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to add satisfied requirements {requirement} to course {course} that doesn't exist");
            }
            await _courses.AddSatisfiedRequirementAsync(course, requirement);
        }

        [HttpDelete("satisfies/{reqId}")]
        public async Task RemoveSatisfiedRequirementAsync(Course course, Requirement requirement)
        {
            if (requirement == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to remove satisfied requirements {requirement} to course {course} that doesn't exist");
            }
            await _courses.RemoveSatisfiedRequirementAsync(course, requirement);
        }
    }
}