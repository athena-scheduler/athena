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
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/student/{id}/programs/{programId}")]
    public class StudentProgramController : Controller
    {
        private readonly IStudentRepository _students;
        private readonly IProgramRepository _programs;

        public StudentProgramController(IStudentRepository studentsRepository, IProgramRepository programsRepository)
        {
            _students = studentsRepository ?? throw new ArgumentNullException(nameof(studentsRepository));
            _programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));
        }

        [HttpPost()]
        public async Task RegisterStudentForProgram(Guid id, Guid programId)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            var program = (await _programs.GetAsync(programId)).NotFoundIfNull();

            await _students.RegisterForProgramAsync(student, program);
        }

        [HttpDelete()]
        public async Task UnregisterForProgram(Guid id, Guid programId)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            var program = (await _programs.GetAsync(programId)).NotFoundIfNull();

            await _students.UnregisterForProgramAsync(student, program);
        }
    }
}