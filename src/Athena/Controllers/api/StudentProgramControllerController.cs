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
    [Route("api/v1/student/{id}/programs/{programId}")]
    public class StudentProgramControllerController : Controller
    {
        private readonly IStudentRepository _students;
        private readonly IProgramRepository _programs;

        public StudentProgramControllerController(IStudentRepository studentsRepository, IProgramRepository programsRepository)
        {
            _students = studentsRepository ?? throw new ArgumentNullException(nameof(studentsRepository));
            _programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));
        }

        [HttpPost()]
        public async Task RegisterStudentForProgram(Student student, Program program)
        {
            if (student == null || program == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to register for program {program}, where the student {student} or program {program} does not exist");
            }
            await _students.RegisterForProgramAsync(student, program);
        }

        [HttpDelete()]
        public async Task UnregisterForProgram(Student student, Program program)
        {
            if (student == null || program == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to register for program {program}, where the student {student} or program {program} does not exist");
            }
            await _students.UnregisterForProgramAsync(student, program);
        }
    }
}