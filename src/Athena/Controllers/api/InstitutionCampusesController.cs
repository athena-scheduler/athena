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
    public class InstitutionCampusesController : Controller
    {
        private readonly ICampusRepository campuses;
        private readonly IInstitutionRepository institutions;

        public InstitutionCampusesController(ICampusRepository campusesRepository, IInstitutionRepository institutionsRepository)
        {
            campuses = campusesRepository ?? throw new ArgumentNullException(nameof(campusesRepository));
            institutions = institutionsRepository ?? throw new ArgumentNullException(nameof(institutionsRepository));
        }

        [HttpGet("api/v1/institution/{id}/campuses")]
        public async Task<IEnumerable<Campus>> GetCampusesForInstitutionAsync(Institution institution)
        {
            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get campus for {institution} that does not exist");
            }
            return (await (campuses.GetCampusesForInstitutionAsync(institution)));
        }

        [HttpPut("api/v1/institution/{id}/campuses/{campusId}")]
        public async Task AssociateCampusWithInstitutionAsync(Campus campus, Institution institution)
        {
            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to assciate {campus} with {institution} that does not exist");
            }
            await (campuses.AssociateCampusWithInstitutionAsync(campus, institution));
        }

        [HttpDelete("api/v1/institution/{id}/campuses/{campusId}")]
        public async Task DissassociateCampusWithInstitutionAsync(Campus campus, Institution institution)
        {
            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to dissassciate {campus} with {institution} that does not exist");
            }
            await (campuses.DissassociateCampusWithInstitutionAsync(campus, institution));
        }
    }
}