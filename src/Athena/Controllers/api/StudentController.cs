using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Exceptions;

namespace Athena.Controllers.api
{
    [Route("api/v1/[Controller]")]

    public class StudentController : Controller
    {

        private readonly IStudentRepository students;
        private readonly IProgramRepository programs;
        public StudentController(IStudentRepository studentsRepository, IProgramRepository programsRepository)
        {
            students = studentsRepository ?? throw new ArgumentNullException(nameof(studentsRepository));
            programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));
           
        }

        [HttpPost]
        public async Task AddStudent([FromBody] Student student) => await students.AddAsync(student);

        [HttpGet("{id}")]
        public async Task<Student> GetStudent(Guid id) => await students.GetAsync(id);

        [HttpPut("{id}")]
        public async Task EditStudent(Guid id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {student.Id}");
            }
            await students.EditAsync(student);
        }

        [HttpDelete("{id}")]
        public async Task DeleteStudent(Guid id)
        {
            var student = await students.GetAsync(id);
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to delete {student} that does not exist");
            }
            await students.DeleteAsync(student);
        }

       public async Task RegisterStudnetForProgram (Student student, Program program)
        {
            if (student == null || program == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to register for program {program}, where the student {student} or program {program} does not exist");
            }
            await students.RegisterForProgramAsync(student, program);
        }

        public async Task UnregisterForProgram (Student student, Program program)
        {
            if (student == null || program == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to register for program {program}, where the student {student} or program {program} does not exist");
            }
            await students.UnregisterForProgramAsync(student, program);
        }

    }
}