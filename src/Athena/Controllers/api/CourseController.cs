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
        public async Task<IEnumerable<Course>> GetCoursesForInstitutionAsync(Institution institution)
        {
            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get course for {institution} that does not exist");
            }
            return (await courses.GetCoursesForInstitutionAsync(institution));
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

        public async Task MarkCourseAsCompletedForStudentAsync(Course course, Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to mark course as completed for {student} that does not exist");
            }
             await courses.MarkCourseAsCompletedForStudentAsync(course, student);
        }

        public async Task MarkCourseAsUncompletedForStudentAsync(Course course, Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to mark course as uncompleted for {student} that does not exist");
            }
            await courses.MarkCourseAsUncompletedForStudentAsync(course, student);
        }

        [HttpGet]
        public async Task<IEnumerable<Course>> GetInProgressCoursesForStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get in-progress for {student} that does not exist");
            }
            return (await courses.GetInProgressCoursesForStudentAsync(student));
        }

        public async Task MarkCourseInProgressForStudentAsync(Course course, Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to mark in-progress courses for {student} that does not exist");
            }
            await courses.MarkCourseInProgressForStudentAsync(course , student);
        }

        public async Task MarkCourseNotInProgressForStudentAsync(Course course, Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to mark not in-progress courses for {student} that does not exist");
            }
            await courses.MarkCourseNotInProgressForStudentAsync(course, student);
        }

        public async Task AddOfferingAsync(Course course, Offering offering)
        {
            if (offering == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add offering {offering} to course {course} that doesn't exist");
            }
            await courses.AddOfferingAsync(course, offering);
        }

        public async Task RemoveOfferingAsync(Course course, Offering offering)
        {
            if (offering == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to remove offering {offering} to course {course} that doesn't exist");
            }
            await courses.RemoveOfferingAsync(course, offering);
        }

        public async Task AddSatisfiedRequirementAsync(Course course, Requirement requirement)
        {
            if (requirement == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add satisfied requirements {requirement} to course {course} that doesn't exist");
            }
            await courses.AddSatisfiedRequirementAsync(course, requirement);
        }

        public async Task RemoveSatisfiedRequirementAsync(Course course, Requirement requirement)
        {
            if (requirement == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to remove satisfied requirements {requirement} to course {course} that doesn't exist");
            }
            await courses.RemoveSatisfiedRequirementAsync(course, requirement);
        }

        public async Task AddPrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add prereq {prereq} to course {course} that doesn't exist");
            }
            await courses.AddPrerequisiteAsync(course, prereq);
        }

        public async Task RemovePrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to remove prereq {prereq} to course {course} that doesn't exist");
            }
            await courses.RemovePrerequisiteAsync(course, prereq);
        }

        public async Task AddConcurrentPrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add concurrent prereq {prereq} to course {course} that doesn't exist");
            }
            await courses.AddConcurrentPrerequisiteAsync(course, prereq);
        }

        public async Task RemoveConcurrentPrerequisiteAsync(Course course, Requirement prereq)
        {
            if (prereq == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to remove concurrent prereq {prereq} to course {course} that doesn't exist");
            }
            await courses.RemoveConcurrentPrerequisiteAsync(course, prereq);
        }
    }
}