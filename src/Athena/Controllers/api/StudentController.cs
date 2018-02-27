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
        private readonly IInstitutionRepository institutions;
        public StudentController(IStudentRepository studentsRepository, IProgramRepository programsRepository, IInstitutionRepository institutionsRepository)
        {
            students = studentsRepository ?? throw new ArgumentNullException(nameof(studentsRepository));
            programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));
            institutions = institutionsRepository ?? throw new ArgumentNullException(nameof(institutionsRepository));
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

        [HttpGet("/{id}/institutions")]
        public async Task<IEnumerable<Institution>> GetInstitutionsForStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Institutions for {student} that does not exist");
            }
            return (await institutions.GetInstitutionsForStudentAsync(student));
        }

        [HttpPost("/{id}/institutions/{institutionId}")]
        public async Task EnrollStudentAsync(Institution institution, Student student)
        {
            if (student == null || institution == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Enroll {student} in Institution {institution} where eihter the student or institution not exist");
            }
            await institutions.EnrollStudentAsync(institution, student);
        }

        [HttpDelete("/{id}/institutions/{institutionId}")]
        public async Task UnenrollStudentAsync(Institution institution, Student student)
        {
            if (institution == null || student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Unenroll {student} in Institution {institution} where eihter the student or institution not exist");
            }
            await institutions.UnenrollStudentAsync(institution, student);
        }

        [HttpGet("/{id}/institutions/{institutionId}")]
        public async Task<IEnumerable<Program>> GetProgramsForStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Programs for {student}, where {student} does not exist");
            }
            return (await programs.GetProgramsForStudentAsync(student));
        }
    }
}