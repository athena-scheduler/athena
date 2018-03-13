using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Exceptions;
using System.Net;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/[Controller]")]
    public class RequirementController : Controller
    {
      private readonly IRequirementRepository _requirements;

        public RequirementController (IRequirementRepository requirementRepository) =>
            _requirements = requirementRepository ?? throw new ArgumentNullException(nameof(requirementRepository));

        [HttpPost]
        public async Task AddRequirement([FromBody] Requirement requirement) => await _requirements.AddAsync(requirement);

        [HttpGet("{id}")]
        public async Task<Requirement> GetRequirement(Guid id) =>
            await _requirements.GetAsync(id) ?? throw new ApiException(HttpStatusCode.NotFound, "requirement not found");

        [HttpPut("{id}")]
        public async Task EditRequirement(Guid id, [FromBody] Requirement requirement)
        {
            if (id != requirement.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {requirement.Id}");
            }
            await _requirements.EditAsync(requirement);
        }

        [HttpDelete("{id}")]
        public async Task DeleteRequirement(Guid id)
        {
            var requirement = (await _requirements.GetAsync(id)).NotFoundIfNull();
            
            await _requirements.DeleteAsync(requirement);
        }
    }
}