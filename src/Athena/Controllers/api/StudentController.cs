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
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/[Controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _students;
        private readonly IProgramRepository _programs;
        private readonly IInstitutionRepository _institutions;
        public StudentController(IStudentRepository studentsRepository, IProgramRepository programsRepository, IInstitutionRepository institutionsRepository)
        {
            _students = studentsRepository ?? throw new ArgumentNullException(nameof(studentsRepository));
            _programs = programsRepository ?? throw new ArgumentNullException(nameof(programsRepository));
            _institutions = institutionsRepository ?? throw new ArgumentNullException(nameof(institutionsRepository));
        }

        [HttpPost]
        public async Task AddStudent([FromBody] Student student) => await _students.AddAsync(student);

        [HttpGet("{id}")]
        public async Task<Student> GetStudent(Guid id) => await _students.GetAsync(id) ?? throw new ApiException(HttpStatusCode.NotFound, "student not found");

        [HttpPut("{id}")]
        public async Task EditStudent(Guid id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {student.Id}");
            }
            await _students.EditAsync(student);
        }

        [HttpDelete("{id}")]
        public async Task DeleteStudent(Guid id)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to delete {student} that does not exist");
            }
            await _students.DeleteAsync(student);
        }

        [HttpGet("{id}/institutions")]
        public async Task<IEnumerable<Institution>> GetInstitutionsForStudentAsync(Guid id)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            
            return await _institutions.GetInstitutionsForStudentAsync(student);
        }

        [HttpPost("{id}/institutions/{institutionId}")]
        public async Task EnrollStudentAsync(Guid institutionId, Guid studentId)
        {
            var institution = (await _institutions.GetAsync(institutionId)).NotFoundIfNull();
            var student = (await _students.GetAsync(studentId)).NotFoundIfNull();

            await _institutions.EnrollStudentAsync(institution, student);
        }

        [HttpDelete("{id}/institutions/{institutionId}")]
        public async Task UnenrollStudentAsync(Guid institutionId, Guid studentId)
        {
            var institution = (await _institutions.GetAsync(institutionId)).NotFoundIfNull();
            var student = (await _students.GetAsync(studentId)).NotFoundIfNull();

            await _institutions.UnenrollStudentAsync(institution, student);
        }

        [HttpGet("{id}/institutions/{institutionId}")]
        public async Task<IEnumerable<Program>> GetProgramsForStudentAsync(Guid id)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();

            return await _programs.GetProgramsForStudentAsync(student);
        }
    }
}