using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/student/{id}/courses")]
    public class StudentCoursesController : AthenaApiController
    {
        private readonly ICourseRepository _courses;
        private readonly IStudentRepository _students;

        public StudentCoursesController(ICourseRepository coursesRepository, IStudentRepository studentRepository)
        {
            _courses = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
            _students = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }

        [HttpGet("completed")]
        public async Task<IEnumerable<Course>> GetCompletedCoursesForStudentAsync(Guid id)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();

            return await _courses.GetCompletedCoursesForStudentAsync(student);
        }

        [HttpPut("completed/{courseId}")]
        public async Task MarkCourseAsCompletedForStudentAsync(Guid id, Guid courseId)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            var course = (await _courses.GetAsync(courseId)).NotFoundIfNull();

            await _courses.MarkCourseAsCompletedForStudentAsync(course, student);
        }

        [HttpDelete("completed/{courseId}")]
        public async Task MarkCourseAsUncompletedForStudentAsync(Guid id, Guid courseId)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            var course = (await _courses.GetAsync(courseId)).NotFoundIfNull();

            await _courses.MarkCourseAsUncompletedForStudentAsync(course, student);
        }

        [HttpGet("in-progress")]
        public async Task<IEnumerable<Course>> GetInProgressCoursesForStudentAsync(Guid id)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();

            return await _courses.GetInProgressCoursesForStudentAsync(student);
        }

        [HttpPut("in-progress/{courseId}")]
        public async Task MarkCourseInProgressForStudentAsync(Guid id, Guid courseId)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            var course = (await _courses.GetAsync(courseId)).NotFoundIfNull();

            await _courses.MarkCourseInProgressForStudentAsync(course, student);
        }

        [HttpDelete("in-progress/{courseId}")]
        public async Task MarkCourseNotInProgressForStudentAsync(Guid id, Guid courseId)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            var course = (await _courses.GetAsync(courseId)).NotFoundIfNull();

            await _courses.MarkCourseNotInProgressForStudentAsync(course, student);
        }
    }
}