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
    [Route("api/v1/program/{id}/requirement")]
    public class RequirementProgramController : Controller
    {
        private readonly IRequirementRepository _requirements;
        private readonly IProgramRepository _programs;

        public RequirementProgramController (IRequirementRepository requirementRepository, IProgramRepository programRepository)
        {
            _requirements = requirementRepository ?? throw new ArgumentNullException(nameof(requirementRepository));
            _programs = programRepository ?? throw new ArgumentNullException(nameof(programRepository));

        }

        [HttpPost()]
        public async Task AddRequirementAsync(Program program, Requirement requirement)
        {
            if (program == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to add Requirements for {program}, where {program} does not exist");
            }
            await _programs.AddRequirementAsync(program, requirement);
        }

        [HttpDelete("{reqId}")]
        public async Task RemoveRequirementAsync(Program program, Requirement requirement)
        {
            if (program == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to remove Requirements for {program}, where {program} does not exist");
            }
            await _programs.RemoveRequirementAsync(program, requirement);
        }
    }
}
