using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Repositories;
using Athena.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers.api
{
    [Route("api/v1/campus/{campusId}/institution")]
    public class CampusInstitutionController : AthenaApiController
    {
        private readonly ICampusRepository _campuses;
        private readonly IInstitutionRepository _institutions;

        public CampusInstitutionController(ICampusRepository campusesRepository, IInstitutionRepository institutionsRepository)
        {
            _campuses = campusesRepository ?? throw new ArgumentNullException(nameof(campusesRepository));
            _institutions = institutionsRepository ?? throw new ArgumentNullException(nameof(institutionsRepository));
        }
        
        [HttpGet]
        public async Task<IEnumerable<Institution>> GetInstitutionsOnCampusAsync(Guid campusId)
        {
            var campus = (await _campuses.GetAsync(campusId)).NotFoundIfNull();

            return await _institutions.GetInstitutionsOnCampusAsync(campus);
        }
    }
}