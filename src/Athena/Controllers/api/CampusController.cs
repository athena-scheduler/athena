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
    public class CampusController : Controller
    {
        private readonly ICampusRepository campuses;
        private readonly IInstitutionRepository institutions;

        public CampusController(ICampusRepository campusesRepository, IInstitutionRepository institutionsRepository)
        {
            campuses = campusesRepository ?? throw new ArgumentNullException(nameof(campusesRepository));
            institutions = institutionsRepository ?? throw new ArgumentNullException(nameof(institutionsRepository));
        }

        [HttpPost]
        public async Task AddCampus([FromBody] Campus campus) => await campuses.AddAsync(campus);

        [HttpGet("{id}")]
        public async Task<Campus> GetCampus(Guid id) => await campuses.GetAsync(id);

        [HttpPut("{id}")]
        public async Task EditCampus(Guid id, [FromBody] Campus campus)
        {
            if (id != campus.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {campus.Id}");
            }
            await campuses.EditAsync(campus);
        }

        [HttpDelete("{id}")]
        public async Task DeleteCampus(Guid id)
        {
            var campus = await campuses.GetAsync(id);
            if (campus == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to delete {campus} that does not exist");
            }
            await campuses.DeleteAsync(campus);
        }

        [HttpGet("api/v1/campus/{id}/institutions")]
        public async Task<IEnumerable<Institution>> GetInstitutionsOnCampusAsync(Campus campus)
        {
            if (campus == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Institutions for {campus} that does not exist");
            }
            return (await institutions.GetInstitutionsOnCampusAsync(campus));
        }
    }
}