using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Models;
using Athena.Core.Repositories;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/institution/{id}/programs")]
    public class InstitutionProgramController : Controller
    {
        private readonly IInstitutionRepository _institutions;
        private readonly IProgramRepository _programs;

        public InstitutionProgramController(IInstitutionRepository institutionRepository, IProgramRepository programsRepository)
        {
            _institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            
            _programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));
        }
        
        [HttpGet]
        public async Task<IEnumerable<Program>> GetProgramsOfferedByInstitutionAsync(Guid id)
        {
            var institution = (await _institutions.GetAsync(id)).NotFoundIfNull();

            return await _programs.GetProgramsOfferedByInstitutionAsync(institution);
        }
    }
}