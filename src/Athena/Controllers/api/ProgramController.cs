using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Exceptions;
using System.Net;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/[Controller]")]
    public class ProgramController : AthenaApiController
    {
        private readonly IProgramRepository _programs;
        
        public ProgramController(IProgramRepository programsRepository) =>
            _programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));

        [HttpPost]
        public async Task AddProgram([FromBody] Program program) => await _programs.AddAsync(program);
        
        [HttpGet]
        public async Task<IEnumerable<Program>> Search(string q, Guid student = default(Guid))
        {
            if ((q?.Length ?? 0) < 3)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Search term must be at least three characters");
            }

            return await _programs.SearchAsync(new ProgramSearchOptions{Query = q, Student = student});
        }

        [HttpGet("{id}")]
        public async Task<Program> GetProgram(Guid id) =>
            (await _programs.GetAsync(id)).NotFoundIfNull();

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
            var program = (await _programs.GetAsync(id)).NotFoundIfNull();

            await _programs.DeleteAsync(program);
        }
    }
}