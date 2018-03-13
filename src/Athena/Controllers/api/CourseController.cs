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
    [Route("api/v1/[Controller]")]
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courses;
        private readonly IInstitutionRepository _institutions;
        private readonly IStudentRepository _students;
        private readonly IRequirementRepository _requirements;

        public CourseController(ICourseRepository coursesRepository, IInstitutionRepository institutionRepository, IStudentRepository studentRepository, IRequirementRepository requirementRepository)
        {
            _courses = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
            _institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            _students = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _requirements = requirementRepository ?? throw new ArgumentNullException(nameof(requirementRepository));
        }

        [HttpPost]
        public async Task AddCourse([FromBody] Course course) => await _courses.AddAsync(course);

        [HttpGet("{id}")]
        public async Task<Course> GetCourse(Guid id) => await _courses.GetAsync(id) ?? throw new ApiException(HttpStatusCode.NotFound, "course not found");

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

        [HttpGet("{id}/requirements")]
        public async Task<IEnumerable<Requirement>> GetRequirementsCourseSatisfiesAsync(Guid id)
        {
            var course =  (await _courses.GetAsync(id)).NotFoundIfNull();

            return await _requirements.GetRequirementsCourseSatisfiesAsync(course);
        }

        [HttpGet("{id}/requirements/prereq")]
        public async Task<IEnumerable<Requirement>> GetPrereqsForCourseAsync(Guid id)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            
            return await _requirements.GetPrereqsForCourseAsync(course);
        }

        [HttpGet("{id}/requirements/concurrent")]
        public async Task<IEnumerable<Requirement>> GetConcurrentPrereqsAsync(Guid id)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            
            return await _requirements.GetConcurrentPrereqsAsync(course);
        }
    }
}