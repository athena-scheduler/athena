using System;
using System.Threading.Tasks;
using System.Net;
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

        public StudentController(IStudentRepository studentsRepository) =>
            _students = studentsRepository ?? throw new ArgumentNullException(nameof(studentsRepository));

        [HttpPost]
        public async Task AddStudent([FromBody] Student student) => await _students.AddAsync(student);

        [HttpGet("{id}")]
        public async Task<Student> GetStudent(Guid id) =>
            (await _students.GetAsync(id)).NotFoundIfNull();

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

            await _students.DeleteAsync(student);
        }
    }
}