using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Exceptions;
using System.Net;

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
            var institution = await _institutions.GetAsync(id);

            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to get campus for {institution} that does not exist");
            }
            return await _campuses.GetCampusesForInstitutionAsync(institution);
        }

        [HttpPut("{campusId}")]
        public async Task AssociateCampusWithInstitutionAsync(Guid campusId, Guid institutionId)
        {
            var campus = await _campuses.GetAsync(campusId);
            var institution = await _institutions.GetAsync(institutionId);

            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to assciate {campus} with {institution} that does not exist");
            }
            await _campuses.AssociateCampusWithInstitutionAsync(campus, institution);
        }

        [HttpDelete("{campusId}")]
        public async Task DissassociateCampusWithInstitutionAsync(Guid campusId, Guid institutionId)
        {
            var campus = await _campuses.GetAsync(campusId);
            var institution = await _institutions.GetAsync(institutionId);

            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to dissassciate {campus} with {institution} that does not exist");
            }
            await _campuses.DissassociateCampusWithInstitutionAsync(campus, institution);
        }
    }
}