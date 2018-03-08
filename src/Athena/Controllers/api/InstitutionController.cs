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
    public class InstitutionController : Controller
    {
        private readonly IInstitutionRepository _institutions;
        private readonly ICampusRepository _campuses;
        private readonly IStudentRepository _students;
        private readonly IProgramRepository _programs;

        public InstitutionController(IInstitutionRepository institutionRepository, ICampusRepository campusRepository, IStudentRepository studentRepository, IProgramRepository programsRepository)
        {
            _institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            _campuses = campusRepository ?? throw new ArgumentNullException(nameof(campusRepository));
            _students = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));
        }

        [HttpPost]
        public async Task AddInstitution([FromBody] Institution institution) => await _institutions.AddAsync(institution);

        [HttpGet("{id}")]
        public async Task<Institution> GetInstitution(Guid id) => await _institutions.GetAsync(id) ?? throw new ApiException(HttpStatusCode.NotFound, "institution not found");

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
            var institution = await _institutions.GetAsync(id);
            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to delete {institution} that does not exist");
            }
            await _institutions.DeleteAsync(institution);
        }

        [HttpGet("{id}/programs")]
        public async Task<IEnumerable<Program>> GetProgramsOfferedByInstitutionAsync(Guid institutionId)
        {
            var institution = await _institutions.GetAsync(institutionId);

            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to get Programs from {institution}, where {institution} does not exist");
            }
            return await _programs.GetProgramsOfferedByInstitutionAsync(institution);
        }
    }
}