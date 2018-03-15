using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/program/{id}/requirement")]
    public class RequirementProgramController : AthenaApiController
    {
        private readonly IRequirementRepository _requirements;
        private readonly IProgramRepository _programs;

        public RequirementProgramController (IRequirementRepository requirementRepository, IProgramRepository programRepository)
        {
            _requirements = requirementRepository ?? throw new ArgumentNullException(nameof(requirementRepository));
            _programs = programRepository ?? throw new ArgumentNullException(nameof(programRepository));
        }
        
        [HttpGet("requirements")]
        public async Task<IEnumerable<Requirement>> GetRequirementsForProgramAsync(Guid id)
        {
            var program = (await _programs.GetAsync(id)).NotFoundIfNull();
           
            return await _requirements.GetRequirementsForProgramAsync(program);
        }

        [HttpPost("{reqId}")]
        public async Task AddRequirementAsync(Guid id, Guid reqId)
        {
            var program = (await _programs.GetAsync(id)).NotFoundIfNull();
            var requirement = (await _requirements.GetAsync(reqId)).NotFoundIfNull();

            await _programs.AddRequirementAsync(program, requirement);
        }

        [HttpDelete("{reqId}")]
        public async Task RemoveRequirementAsync(Guid id, Guid reqId)
        {
            var program = (await _programs.GetAsync(id)).NotFoundIfNull();
            var requirement = (await _requirements.GetAsync(reqId)).NotFoundIfNull();

            await _programs.RemoveRequirementAsync(program, requirement);
        }
    }
}
