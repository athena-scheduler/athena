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
    public class ProgramController : Controller
    {
        private readonly IProgramRepository _programs;
        private readonly IInstitutionRepository _institutions;
        private readonly IStudentRepository _students;
        private readonly IRequirementRepository _requirements;
        
        public ProgramController(IProgramRepository programsRepository, IInstitutionRepository institutionRepository, IStudentRepository studentRepository, IRequirementRepository requirementRepository)
        {
            _programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));
            _institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            _students = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _requirements = requirementRepository ?? throw new ArgumentNullException(nameof(requirementRepository));
        }

        [HttpPost]
        public async Task AddProgram([FromBody] Program program) => await _programs.AddAsync(program);

        [HttpGet("{id}")]
        public async Task<Program> GetProgram(Guid id) => await _programs.GetAsync(id) ?? throw new ApiException(HttpStatusCode.NotFound, "program not found");

        [HttpPut("{id}")]
        public async Task EditProgram (Guid id, [FromBody] Program program)
        {
            if (id != program.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {program.Id}");
            }
            await _programs.EditAsync(program);
        }

        [HttpDelete("{id}")]
        public async Task DeleteProgram(Guid id)
        {
            var program = await _programs.GetAsync(id);
            if (program == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to delete {program} that does not exist");
            }
            await _programs.DeleteAsync(program);
        }

        [HttpGet("{id}/requirements")]
        public async Task<IEnumerable<Requirement>> GetRequirementsForProgramAsync(Guid id)
        {
            var program = await _programs.GetAsync(id);
            if (program == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"program with id {id} not found");
            }
            return await _requirements.GetRequirementsForProgramAsync(program);
        }
    }
}