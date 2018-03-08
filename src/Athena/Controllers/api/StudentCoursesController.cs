using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Exceptions;
using System.Net;

namespace Athena.Controllers.api
{
    [Route("api/v1/student/{id}")]
    public class StudentCoursesController : Controller
    {
        private readonly ICourseRepository _courses;
        private readonly IStudentRepository _students;

        public StudentCoursesController(ICourseRepository coursesRepository, IStudentRepository studentRepository)
        {
            _courses = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
            _students = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }

        [HttpGet("courses/completed")]
        public async Task<IEnumerable<Course>> GetCompletedCoursesForStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get course for {student} that does not exist");
            }
            return await _courses.GetCompletedCoursesForStudentAsync(student);
        }

        [HttpPost("api/v1/student/{id}/courses/completed/{courseId}")]
        public async Task MarkCourseAsCompletedForStudentAsync(Course course, Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to mark course as completed for {student} that does not exist");
            }
            await _courses.MarkCourseAsCompletedForStudentAsync(course, student);
        }

        [HttpDelete("api/v1/student/{id}/courses/completed/{courseId}")]
        public async Task MarkCourseAsUncompletedForStudentAsync(Course course, Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to mark course as uncompleted for {student} that does not exist");
            }
            await _courses.MarkCourseAsUncompletedForStudentAsync(course, student);
        }

        [HttpGet("api/v1/student/{id}/courses/in-progress")]
        public async Task<IEnumerable<Course>> GetInProgressCoursesForStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get in-progress for {student} that does not exist");
            }
            return await _courses.GetInProgressCoursesForStudentAsync(student);
        }

        [HttpPost("api/v1/student/{id}/courses/in-progress/{courseId}")]
        public async Task MarkCourseInProgressForStudentAsync(Course course, Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to mark in-progress courses for {student} that does not exist");
            }
            await _courses.MarkCourseInProgressForStudentAsync(course, student);
        }

        [HttpDelete("api/v1/student/{id}/courses/in-progress/{courseId}")]
        public async Task MarkCourseNotInProgressForStudentAsync(Course course, Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to mark not in-progress courses for {student} that does not exist");
            }
            await _courses.MarkCourseNotInProgressForStudentAsync(course, student);
        }
    }
}