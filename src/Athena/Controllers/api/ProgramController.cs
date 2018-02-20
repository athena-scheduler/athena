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

        [HttpGet]
        public async Task<IEnumerable<Program>> GetProgramsOfferedByInstitutionAsync(Institution institution)
        {
            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Programs from {institution}, where {institution} does not exist");
            }
            return (await programs.GetProgramsOfferedByInstitutionAsync(institution));
        }
        
        [HttpGet]
        public async Task<IEnumerable<Program>> GetProgramsForStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Programs for {student}, where {student} does not exist");
            }
            return (await programs.GetProgramsForStudentAsync(student));
        }

        public async Task AddRequirementAsync(Program program, Requirement requirement)
        {
            if (program == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add Requirements for {program}, where {program} does not exist");
            }
            await programs.AddRequirementAsync(program, requirement);
        }

        public async Task RemoveRequirementAsync(Program program, Requirement requirement)
        {
            if (program == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to remove Requirements for {program}, where {program} does not exist");
            }
            await programs.RemoveRequirementAsync(program, requirement);
        }
    }
}