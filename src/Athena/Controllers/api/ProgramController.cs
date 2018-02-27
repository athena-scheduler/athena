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
        private readonly IProgramRepository programs;
        private readonly IInstitutionRepository institutions;
        private readonly IStudentRepository students;
        private readonly IRequirementRepository requirements;
        
        public ProgramController(IProgramRepository programsRepository, IInstitutionRepository institutionRepository, IStudentRepository studentRepository, IRequirementRepository requirementRepository)
        {
            programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));
            institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            students = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            requirements = requirementRepository ?? throw new ArgumentNullException(nameof(requirementRepository));
        }

        [HttpPost]
        public async Task AddProgram([FromBody] Program program) => await programs.AddAsync(program);

        [HttpGet("{id}")]
        public async Task<Program> GetProgram(Guid id) => await programs.GetAsync(id);

        [HttpPut("{id}")]
        public async Task EditProgram (Guid id, [FromBody] Program program)
        {
            if (id != program.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {program.Id}");
            }
            await programs.EditAsync(program);
        }

        [HttpDelete("{id}")]
        public async Task DeleteProgram(Guid id)
        {
            var program = await programs.GetAsync(id);
            if (program == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to delete {program} that does not exist");
            }
            await programs.DeleteAsync(program);
        }

        [HttpGet("{id}/requirements")]
        public async Task<IEnumerable<Requirement>> GetRequirementsForProgramAsync(Guid id)
        {
            var program = await programs.GetAsync(id);
            if (program == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"program with id {id} not found");
            }
            return (await requirements.GetRequirementsForProgramAsync(program));
        }

        [HttpPost("api/v1/student/{id}/programs/{programId}")]
        public async Task RegisterStudnetForProgram(Student student, Program program)
        {
            if (student == null || program == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to register for program {program}, where the student {student} or program {program} does not exist");
            }
            await students.RegisterForProgramAsync(student, program);
        }

        [HttpDelete("api/v1/student/{id}/programs/{programId}")]
        public async Task UnregisterForProgram(Student student, Program program)
        {
            if (student == null || program == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to register for program {program}, where the student {student} or program {program} does not exist");
            }
            await students.UnregisterForProgramAsync(student, program);
        }
    }
}