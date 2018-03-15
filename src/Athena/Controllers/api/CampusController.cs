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
    public class CampusController : AthenaApiController
    {
        private readonly ICampusRepository _campuses;

        public CampusController(ICampusRepository campusesRepository) =>
            _campuses = campusesRepository ?? throw new ArgumentNullException(nameof(campusesRepository));

        [HttpPost]
        public async Task AddCampus([FromBody] Campus campus) => await _campuses.AddAsync(campus);

        [HttpGet("{id}")]
        public async Task<Campus> GetCampus(Guid id) => (await _campuses.GetAsync(id)).NotFoundIfNull();

        [HttpPut("{id}")]
        public async Task EditCampus(Guid id, [FromBody] Campus campus)
        {
            if (id != campus.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {campus.Id}");
            }
            await _campuses.EditAsync(campus);
        }

        [HttpDelete("{id}")]
        public async Task DeleteCampus(Guid id)
        {
            var campus = (await _campuses.GetAsync(id)).NotFoundIfNull();
            
            await _campuses.DeleteAsync(campus);
        }
    }
}