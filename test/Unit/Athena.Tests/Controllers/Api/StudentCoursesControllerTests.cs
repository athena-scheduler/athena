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

        public StudentCoursesControllerTests() => _controller = new StudentCoursesController(Coureses.Object, Students.Object);

        [Theory, AutoData]
        public async Task GetCompletedCoursesForStudent_Valid (Student student)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);

            await _controller.GetCompletedCoursesForStudentAsync(student);

            Coureses.Verify(c => c.GetCompletedCoursesForStudentAsync(student), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetCompletedCoursesForStudent_ThrowsforNullStudent(Student student)
        {
            

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetCompletedCoursesForStudentAsync(null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task MarkCourseAsCompletedForStudent_Valid(Course course , Student student)
        {
           
            await _controller.MarkCourseAsCompletedForStudentAsync(course, student);

            Coureses.Verify(c => c.MarkCourseAsCompletedForStudentAsync(course , student), Times.Once);
        }

        [Theory, AutoData]
        public async Task MarkCourseAsCompletedForStudent_ThrowsforNullStudent(Course course)
        {
    
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.MarkCourseAsCompletedForStudentAsync(course , null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task MarkCourseAsUncompletedForStudentAsync_Valid(Course course, Student student)
        {

            await _controller.MarkCourseAsUncompletedForStudentAsync(course, student);

            Coureses.Verify(c => c.MarkCourseAsUncompletedForStudentAsync(course, student), Times.Once);
        }

        [Theory, AutoData]
        public async Task MarkCourseAsUncompletedForStudentAsync_ThrowsforNullStudent(Course course)
        {

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.MarkCourseAsUncompletedForStudentAsync(course, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetInProgressCoursesForStudent_Valid(Student student)
        {
            Students.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);

            await _controller.GetInProgressCoursesForStudentAsync(student);

            Coureses.Verify(c => c.GetInProgressCoursesForStudentAsync(student), Times.Once);
        }

        [Fact]
        public async Task GetInProgressCoursesForStudent_ThrowsforNullStudent()
        {
            

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetInProgressCoursesForStudentAsync(null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task MarkCourseInProgressForStudentAsync_Valid(Course course, Student student)
        {

            await _controller.MarkCourseInProgressForStudentAsync(course, student);

            Coureses.Verify(c => c.MarkCourseInProgressForStudentAsync(course, student), Times.Once);
        }

        [Theory, AutoData]
        public async Task MarkCourseInProgressForStudentAsync_ThrowsforNullStudent(Course course)
        {

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.MarkCourseInProgressForStudentAsync(course, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task MarkCourseNotInProgressForStudentAsync_Valid(Course course, Student student)
        {

            await _controller.MarkCourseNotInProgressForStudentAsync(course, student);

            Coureses.Verify(c => c.MarkCourseNotInProgressForStudentAsync(course, student), Times.Once);
        }

        [Theory, AutoData]
        public async Task MarkCourseNotInProgressForStudentAsync_ThrowsforNullStudent(Course course)
        {

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.MarkCourseNotInProgressForStudentAsync(course, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }
    }
}
