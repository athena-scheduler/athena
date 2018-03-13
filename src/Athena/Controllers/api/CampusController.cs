﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Exceptions;
using System.Net;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/[Controller]")]
    public class CampusController : Controller
    {
        private readonly ICampusRepository _campuses;
        private readonly IInstitutionRepository _institutions;

        public CampusController(ICampusRepository campusesRepository, IInstitutionRepository institutionsRepository)
        {
            _campuses = campusesRepository ?? throw new ArgumentNullException(nameof(campusesRepository));
            _institutions = institutionsRepository ?? throw new ArgumentNullException(nameof(institutionsRepository));
        }

        [HttpPost]
        public async Task AddCampus([FromBody] Campus campus) => await _campuses.AddAsync(campus);

        [HttpGet("{id}")]
        public async Task<Campus> GetCampus(Guid id) => (await _campuses.GetAsync(id)) ?? throw new ApiException(HttpStatusCode.NotFound, "campus not found");

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

        [HttpGet("{id}/institutions")]
        public async Task<IEnumerable<Institution>> GetInstitutionsOnCampusAsync(Guid id)
        {
            var campus = (await _campuses.GetAsync(id)).NotFoundIfNull();

            return await _institutions.GetInstitutionsOnCampusAsync(campus);
        }
    }
}