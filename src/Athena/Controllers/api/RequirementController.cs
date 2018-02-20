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
        
        [HttpGet]
        public async Task<IEnumerable<Requirement>> GetRequirementsCourseSatisfiesAsync(Course course)
        {
            if (course == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get requirements {course} satisfies for {course} that doesn't exist");
            }
            return (await requirements.GetRequirementsCourseSatisfiesAsync(course));
        }

        public async Task<IEnumerable<Requirement>> GetPrereqsForCourseAsync(Course course)
        {
            if (course == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get prereq requirements for {course} that doesn't exist");
            }
            return (await requirements.GetPrereqsForCourseAsync(course));
        }

        [HttpGet]
        public async Task<IEnumerable<Requirement>> GetConcurrentPrereqsAsync(Course course)
        {
            if (course == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get concurrent prereq requirements for {course} that doesn't exist");
            }
            return (await requirements.GetConcurrentPrereqsAsync(course));
        }

        [HttpGet]
        public async Task<IEnumerable<Requirement>> GetRequirementsForProgramAsync(Program program)
        {
            if (program == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get requirements for {program} that doesn't exist");
            }
            return (await GetRequirementsForProgramAsync(program));
        }

    }
}