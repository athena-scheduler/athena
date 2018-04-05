using Athena.Controllers.api;
using System;
using System.Collections.Generic;
using System.Text;
using Athena.Core.Models;
using Athena.Exceptions;
using AutoFixture.Xunit2;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Athena.Tests.Extensions;
using Athena.Core.Repositories;
using System.Linq;

namespace Athena.Tests.Controllers.Api
{
    public class StudentCoursesControllerTests : ControllerTest
    {
        private readonly StudentCoursesController _controller;

        public StudentCoursesControllerTests() => _controller = new StudentCoursesController(Courses.Object, Students.Object);

        [Theory, AutoData]
        public async Task GetCompletedCoursesForStudent_Valid (Guid studentId,Student student, List<Course> courses)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Courses.Setup(c => c.GetCompletedCoursesForStudentAsync(It.IsAny<Student>())).ReturnsAsync(courses);

            var result = (await _controller.GetCompletedCoursesForStudentAsync(studentId)).ToList();

            Assert.Equal(courses.Count, result.Count);
            Assert.All(courses, c => Assert.Contains(c, result));

            Courses.Verify(c => c.GetCompletedCoursesForStudentAsync(student), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetCompletedCoursesForStudent_ThrowsforNullStudent(Guid studentId)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetCompletedCoursesForStudentAsync(studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetCompletedCoursesForStudentAsync_SearchValid(List<Course> courses, Student student, string query)
        {
            Students.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Courses.Setup(c => c.SearchAsync(It.IsAny<CourseSearchOptions>())).ReturnsAsync(courses);

            var result = (await _controller.GetCompletedCoursesForStudentAsync(student.Id, query)).ToList();

            Assert.Equal(courses.Count, result.Count);
            Assert.All(courses, c => Assert.Contains(c, result));

            Courses.Verify(c => c.SearchAsync(new CourseSearchOptions { StudentId = student.Id, Completed = true, Query = query }), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetIncompleteCoursesForStudentAsync_SearchValid(List<Course> courses, Student student, string query)
        {
            Students.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Courses.Setup(c => c.SearchAsync(It.IsAny<CourseSearchOptions>())).ReturnsAsync(courses);

            var result = (await _controller.GetIncompleteCourses(student.Id, query)).ToList();

            Assert.Equal(courses.Count, result.Count);
            Assert.All(courses, c => Assert.Contains(c, result));

            Courses.Verify(c => c.SearchAsync(new CourseSearchOptions { StudentId = student.Id, Completed = false, Query = query }), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetIncompleteCoursesForStudent_ThrowsforNullStudent(Guid studentId, string query)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetIncompleteCourses(studentId, query));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task MarkCourseAsCompletedForStudent_Valid(Guid studentId, Guid courseId, Course course , Student student)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Courses.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            await _controller.MarkCourseAsCompletedForStudentAsync(courseId, studentId);

            Courses.Verify(c => c.MarkCourseAsCompletedForStudentAsync(course , student), Times.Once);
        }

        [Theory, AutoData]
        public async Task MarkCourseAsCompletedForStudent_ThrowsforNullStudent(Guid studentId, Guid courseId)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Courses.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.MarkCourseAsCompletedForStudentAsync(courseId , studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task MarkCourseAsUncompletedForStudentAsync_Valid(Guid courseId , Guid studentId ,Course course, Student student)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Courses.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            await _controller.MarkCourseAsUncompletedForStudentAsync(courseId, studentId);

            Courses.Verify(c => c.MarkCourseAsUncompletedForStudentAsync(course, student), Times.Once);
        }

        [Theory, AutoData]
        public async Task MarkCourseAsUncompletedForStudentAsync_ThrowsforNullStudent(Guid studentId, Guid courseId)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Courses.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.MarkCourseAsUncompletedForStudentAsync(courseId, studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        
    }
}
