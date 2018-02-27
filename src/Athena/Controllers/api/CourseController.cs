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
    [Route("api/v1/[Controller]")]
    public class CourseController : Controller
    {
        private readonly ICourseRepository courses;
        private readonly IInstitutionRepository institutions;
        private readonly IStudentRepository students;
        private readonly IRequirementRepository requirements;

        public CourseController(ICourseRepository coursesRepository, IInstitutionRepository institutionRepository, IStudentRepository studentRepository, IRequirementRepository requirementRepository)
        {
            courses = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
            institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            students = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            requirements = requirementRepository ?? throw new ArgumentNullException(nameof(requirementRepository));
        }

        [HttpPost]
        public async Task AddCourse([FromBody] Course course) => await courses.AddAsync(course);

        [HttpGet("{id}")]
        public async Task<Course> GetCourse(Guid id) => await courses.GetAsync(id);

        [HttpPut("{id}")]
        public async Task EditCourse(Guid id, [FromBody] Course course)
        {
            if (id != course.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {course.Id}");
            }
            await courses.EditAsync(course);
        }

        [HttpDelete("{id}")]
        public async Task DeleteCourse(Guid id)
        {
            var course = await courses.GetAsync(id);
            if (course == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to delete {course} that does not exist");
            }
            await courses.DeleteAsync(course);
        }

        [HttpGet]
        public async Task<IEnumerable<Course>> GetCompletedCoursesForStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get course for {student} that does not exist");
            }
            return (await courses.GetCompletedCoursesForStudentAsync(student));
        }

        [HttpGet("{id}/requirements")]
        public async Task<IEnumerable<Requirement>> GetRequirementsCourseSatisfiesAsync(Guid id)
        {
            var course =  await courses.GetAsync(id);
            if (course == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"course with id {id} not found");
            }
            return (await requirements.GetRequirementsCourseSatisfiesAsync(course));
        }

        [HttpGet("{id}/requirements")]
        public async Task<IEnumerable<Requirement>> GetPrereqsForCourseAsync(Guid id)
        {
            var course = await courses.GetAsync(id);
            if (course == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"course with id {id} not found");
            }
            return (await requirements.GetPrereqsForCourseAsync(course));
        }

        [HttpGet("{id}/requirements")]
        public async Task<IEnumerable<Requirement>> GetConcurrentPrereqsAsync(Guid id)
        {
            var course = await courses.GetAsync(id);
            if (course == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"course with id {id} not found");
            }
            return (await requirements.GetConcurrentPrereqsAsync(course));
        }
    }
}