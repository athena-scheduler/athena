using Athena.Controllers.api;
using Athena.Core.Models;
using Athena.Exceptions;
using Athena.Tests.Extensions;
using AutoFixture.Xunit2;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Athena.Tests.Controllers.Api
{
    public class CourseControllerTests : ControllerTest
    {
        private readonly CourseController _controller;

        public CourseControllerTests() => _controller = new CourseController(Courses.Object);

        [Theory, AutoData]
        public async Task Add_Valid(Course course)
        {
            await _controller.AddCourse(course);

            Courses.Verify(c => c.AddAsync(course), Times.Once);
        }

        [Theory, AutoData]
        public async Task Get_Valid(Course course)
        {
            Courses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            var result = await _controller.GetCourse(course.Id);

            Assert.Equal(course, result);
        }

        [Theory, AutoData]
        public async Task Get_NotFoundForNull(Guid id)
        {
            Courses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetCourse(id));
            
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task Edit_Valid(Course course)
        {
            await _controller.EditCourse(course.Id, course);

            Courses.Verify(c => c.EditAsync(course), Times.Once);
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
            Courses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            await _controller.DeleteCourse(course.Id);

            Courses.Verify(c => c.DeleteAsync(course), Times.Once);
        }

        [Theory, AutoData]
        public async Task Delete_ThrowsforNullCourse(Guid id)
        {
            Courses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteCourse(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
