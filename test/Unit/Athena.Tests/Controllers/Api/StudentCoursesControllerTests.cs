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

namespace Athena.Tests.Controllers.Api
{
    public class StudentCoursesControllerTests : ControllerTest
    {
        private readonly StudentCoursesController _controller;

        public StudentCoursesControllerTests() => _controller = new StudentCoursesController(Courses.Object, Students.Object);

        [Theory, AutoData]
        public async Task GetCompletedCoursesForStudent_Valid (Guid studentId,Student student)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            
            await _controller.GetCompletedCoursesForStudentAsync(studentId);

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

        [Theory, AutoData]
        public async Task GetInProgressCoursesForStudent_Valid(Guid studentId, Student student)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);

            await _controller.GetInProgressCoursesForStudentAsync(studentId);

            Courses.Verify(c => c.GetInProgressCoursesForStudentAsync(student), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetInProgressCoursesForStudent_ThrowsforNullStudent(Guid studentId)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetInProgressCoursesForStudentAsync(studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task MarkCourseInProgressForStudentAsync_Valid(Guid courseId, Guid studentId, Course course, Student student)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Courses.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            await _controller.MarkCourseInProgressForStudentAsync(courseId, studentId);

            Courses.Verify(c => c.MarkCourseInProgressForStudentAsync(course, student), Times.Once);
        }

        [Theory, AutoData]
        public async Task MarkCourseInProgressForStudentAsync_ThrowsforNullStudent(Guid courseId, Guid studentId)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Courses.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.MarkCourseInProgressForStudentAsync(courseId, studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task MarkCourseNotInProgressForStudentAsync_Valid(Guid courseId, Guid studentId, Course course, Student student)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Courses.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            await _controller.MarkCourseNotInProgressForStudentAsync(courseId, studentId);

            Courses.Verify(c => c.MarkCourseNotInProgressForStudentAsync(course, student), Times.Once);
        }

        [Theory, AutoData]
        public async Task MarkCourseNotInProgressForStudentAsync_ThrowsforNullStudent(Guid courseId , Guid studentId)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Courses.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.MarkCourseNotInProgressForStudentAsync(courseId, studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
