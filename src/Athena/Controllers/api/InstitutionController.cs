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
    public class InstitutionController : Controller
    {
        private readonly IInstitutionRepository _institutions;

        public InstitutionController(IInstitutionRepository institutionRepository) =>
            _institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));

        [HttpPost]
        public async Task AddInstitution([FromBody] Institution institution) => await _institutions.AddAsync(institution);

        [HttpGet("{id}")]
        public async Task<Institution> GetInstitution(Guid id) =>
            await _institutions.GetAsync(id) ?? throw new ApiException(HttpStatusCode.NotFound, "institution not found");

        [HttpPut("{id}")]
        public async Task EditInstitution(Guid id, [FromBody] Institution institution)
        {
            if (id != institution.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {institution.Id}");
            }
            await _institutions.EditAsync(institution);
        }

        [HttpDelete("{id}")]
        public async Task DeleteInstitution(Guid id)
        {
            var institution = (await _institutions.GetAsync(id)).NotFoundIfNull();
           
            await _institutions.DeleteAsync(institution);
        }
    }
}