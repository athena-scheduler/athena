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
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courses;

        public CourseController(ICourseRepository coursesRepository) =>
            _courses = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));

        [HttpPost]
        public async Task AddCourse([FromBody] Course course) => await _courses.AddAsync(course);

        [HttpGet("{id}")]
        public async Task<Course> GetCourse(Guid id) =>
            (await _courses.GetAsync(id)).NotFoundIfNull();

        [HttpPut("{id}")]
        public async Task EditCourse(Guid id, [FromBody] Course course)
        {
            if (id != course.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {course.Id}");
            }
            await _courses.EditAsync(course);
        }

        [HttpDelete("{id}")]
        public async Task DeleteCourse(Guid id)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            
            await _courses.DeleteAsync(course);
        }
    }
}