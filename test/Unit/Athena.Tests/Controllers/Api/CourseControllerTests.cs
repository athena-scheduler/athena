using Athena.Controllers.api;
using Athena.Core.Models;
using Athena.Exceptions;
using Athena.Tests.Extensions;
using AutoFixture.Xunit2;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Athena.Tests.Controllers.Api
{
    public class CourseControllerTests : ControllerTest
    {
        private readonly CourseController _controller;

        public CourseControllerTests() => _controller = new CourseController(Coureses.Object, Institutions.Object, Students.Object, Requirements.Object);

        [Theory, AutoData]
        public async Task Add_valid(Course course)
        {
            await _controller.AddCourse(course);

            Coureses.Verify(c => c.AddAsync(course), Times.Once);
        }

        [Theory, AutoData]
        public async Task Get_Valid(Course course)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            var result = await _controller.GetCourse(course.Id);

            Assert.Equal(course, result);
        }

        [Theory, AutoData]
        public async Task Edit_Valid(Course course)
        {
            await _controller.EditCourse(course.Id, course);

            Coureses.Verify(c => c.EditAsync(course), Times.Once);
        }

        [Theory, AutoData]
        public async Task Edit_ThrowsForMismatchedId(Guid id, Course course)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EditCourse(id, course));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task Delete_Valid(Course course)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            await _controller.DeleteCourse(course.Id);

            Coureses.Verify(c => c.DeleteAsync(course), Times.Once);
        }

        [Theory, AutoData]
        public async Task Delete_ThrowsforNullCourse(Guid id)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Course));

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteCourse(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        

        [Theory, AutoData]
        public async Task GetRequirementsCourseSatisfies_Valid(Course course)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            await _controller.GetRequirementsCourseSatisfiesAsync(course.Id);

            Requirements.Verify(c => c.GetRequirementsCourseSatisfiesAsync(course), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetRequirementsCourseSatisfies_ThrowsforNullCourse(Guid id)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetRequirementsCourseSatisfiesAsync(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetPrereqsForCourse(Course course)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            await _controller.GetPrereqsForCourseAsync(course.Id);

            Requirements.Verify(c => c.GetPrereqsForCourseAsync(course), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetPrereqsForCourse_ThrowsforNullCourse(Guid id)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetPrereqsForCourseAsync(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetConcurrentPrereqs(Course course)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            await _controller.GetConcurrentPrereqsAsync(course.Id);

            Requirements.Verify(c => c.GetConcurrentPrereqsAsync(course), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetConcurrentPrereqs_ThrowsforNullCourse(Guid id)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetConcurrentPrereqsAsync(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
