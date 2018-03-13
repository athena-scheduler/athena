﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Exceptions;
using System.Net;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/institution/{id}/campuses")]
    public class InstitutionCampusesController : Controller
    {
        private readonly ICampusRepository _campuses;
        private readonly IInstitutionRepository _institutions;

        public InstitutionCampusesController(ICampusRepository campusesRepository, IInstitutionRepository institutionsRepository)
        {
            _campuses = campusesRepository ?? throw new ArgumentNullException(nameof(campusesRepository));
            _institutions = institutionsRepository ?? throw new ArgumentNullException(nameof(institutionsRepository));
        }

        [HttpGet]
        public async Task<IEnumerable<Campus>> GetCampusesForInstitutionAsync(Guid id)
        {
            var institution = (await _institutions.GetAsync(id)).NotFoundIfNull();

            return await _campuses.GetCampusesForInstitutionAsync(institution);
        }

       

        [HttpPut("{campusId}")]
        public async Task AssociateCampusWithInstitutionAsync(Guid campusId, Guid id)
        {
            var campus = (await _campuses.GetAsync(campusId)).NotFoundIfNull();
            var institution = (await _institutions.GetAsync(id)).NotFoundIfNull();

            await _campuses.AssociateCampusWithInstitutionAsync(campus, institution);
        }

        [HttpDelete("{campusId}")]
        public async Task DissassociateCampusWithInstitutionAsync(Guid campusId, Guid id)
        {
            var campus = (await _campuses.GetAsync(campusId)).NotFoundIfNull();
            var institution = (await _institutions.GetAsync(id)).NotFoundIfNull();

            await _campuses.DissassociateCampusWithInstitutionAsync(campus, institution);
        }
    }
}