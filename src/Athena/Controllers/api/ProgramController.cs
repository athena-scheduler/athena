using System;
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
    public class ProgramController : Controller
    {
        private readonly IProgramRepository _programs;
        
        public ProgramController(IProgramRepository programsRepository) =>
            _programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));

        [HttpPost]
        public async Task AddProgram([FromBody] Program program) => await _programs.AddAsync(program);

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