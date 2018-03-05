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
        private readonly IInstitutionRepository institutions;
        private readonly ICampusRepository campuses;
        private readonly IStudentRepository students;
        private readonly IProgramRepository programs;

        public InstitutionController(IInstitutionRepository institutionRepository, ICampusRepository campusRepository, IStudentRepository studentRepository, IProgramRepository programsRepository)
        {
            institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            campuses = campusRepository ?? throw new ArgumentNullException(nameof(campusRepository));
            students = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));
        }

        [HttpPost]
        public async Task AddInstitution([FromBody] Institution institution) => await institutions.AddAsync(institution);

        [HttpGet("{id}")]
        public async Task<Institution> GetInstitution(Guid id) => await institutions.GetAsync(id);

        [HttpPut("{id}")]
        public async Task EditInstitution(Guid id, [FromBody] Institution institution)
        {
            if (id != institution.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {institution.Id}");
            }
            await institutions.EditAsync(institution);
        }

        [HttpDelete("{id}")]
        public async Task DeleteInstitution(Guid id)
        {
            var institution = await institutions.GetAsync(id);
            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to delete {institution} that does not exist");
            }
            await institutions.DeleteAsync(institution);
        }

        [HttpGet("/{id}/programs")]
        public async Task<IEnumerable<Program>> GetProgramsOfferedByInstitutionAsync(Institution institution)
        {
            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Programs from {institution}, where {institution} does not exist");
            }
            return (await programs.GetProgramsOfferedByInstitutionAsync(institution));
        }
    }
}