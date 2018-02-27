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
    [Route("api/v1/[Controller]")]
    public class RequirementController : Controller
    {
      private readonly IRequirementRepository requirements;
      private readonly IProgramRepository programs;
      private readonly ICourseRepository courses;

        public RequirementController (IRequirementRepository requirementRepository, IProgramRepository programsRepository, ICourseRepository courseRepository)
        {
            requirements = requirementRepository ?? throw new ArgumentNullException(nameof(requirementRepository));
            programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));
            courses = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        }

        [HttpPost]
        public async Task AddRequirement([FromBody] Requirement requirement) => await requirements.AddAsync(requirement);

        [HttpGet("{id}")]
        public async Task<Requirement> GetRequirement(Guid id) => await requirements.GetAsync(id);

        [HttpPut("{id}")]
        public async Task EditRequirement(Guid id, [FromBody] Requirement requirement)
        {
            if (id != requirement.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {requirement.Id}");
            }
            await requirements.EditAsync(requirement);
        }

        [HttpDelete("{id}")]
        public async Task DeleteRequirement(Guid id)
        {
            var requirement = await requirements.GetAsync(id);
            if (requirement == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to delete {requirement} that does not exist");
            }
            await requirements.DeleteAsync(requirement);
        }

        [HttpPost("api/v1/course/{id}/requirement/satisfies/{reqId}")]
        public async Task AddSatisfiedRequirementAsync(Course course, Requirement requirement)
        {
            if (requirement == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add satisfied requirements {requirement} to course {course} that doesn't exist");
            }
            await courses.AddSatisfiedRequirementAsync(course, requirement);
        }

        [HttpDelete("api/v1/course/{id}/requirement/satisfies/{reqId}")]
        public async Task RemoveSatisfiedRequirementAsync(Course course, Requirement requirement)
        {
            if (requirement == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to remove satisfied requirements {requirement} to course {course} that doesn't exist");
            }
            await courses.RemoveSatisfiedRequirementAsync(course, requirement);
        }

        [HttpPost("api/v1/course/{id}/requirement/prereq/{reqId}")]
        public async Task AddPrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add prereq {prereq} to course {course} that doesn't exist");
            }
            await courses.AddPrerequisiteAsync(course, prereq);
        }

        [HttpDelete("api/v1/course/{id}/requirement/prereq/{reqId}")]
        public async Task RemovePrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to remove prereq {prereq} to course {course} that doesn't exist");
            }
            await courses.RemovePrerequisiteAsync(course, prereq);
        }

        [HttpPost("api/v1/course/{id}/requirement/prereq/concurrent/{reqId}")]
        public async Task AddConcurrentPrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add concurrent prereq {prereq} to course {course} that doesn't exist");
            }
            await courses.AddConcurrentPrerequisiteAsync(course, prereq);
        }

        [HttpDelete("api/v1/course/{id}/requirement/prereq/concurrent/{reqId}")]
        public async Task RemoveConcurrentPrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to remove concurrent prereq {prereq} to course {course} that doesn't exist");
            }
            await courses.RemoveConcurrentPrerequisiteAsync(course, prereq);
        }

        [HttpPost("api/v1/program/{id}/requirement")]
        public async Task AddRequirementAsync(Program program, Requirement requirement)
        {
            if (program == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add Requirements for {program}, where {program} does not exist");
            }
            await programs.AddRequirementAsync(program, requirement);
        }

        [HttpDelete("api/v1/program/{id}/requirement/{reqId}")]
        public async Task RemoveRequirementAsync(Program program, Requirement requirement)
        {
            if (program == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to remove Requirements for {program}, where {program} does not exist");
            }
            await programs.RemoveRequirementAsync(program, requirement);
        }

    }
}