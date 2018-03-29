using Athena.Controllers.api;
using System;
using Athena.Core.Models;
using Athena.Exceptions;
using AutoFixture.Xunit2;
using Moq;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using Athena.Tests.Extensions;

namespace Athena.Tests.Controllers.Api
{
    public class OfferingCourseControllerTests : ControllerTest
    {

        private readonly OfferingCourseController _controller;

        public OfferingCourseControllerTests() => _controller = new OfferingCourseController(Offerings.Object, Courses.Object);


        [Theory, AutoData]
        public async Task GetOfferingsForCourse(Guid courseId , Course course)
        {
            Courses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);

            await _controller.GetOfferingsForCourseAsync(courseId);

            Offerings.Verify(o => o.GetOfferingsForCourseAsync(course));
        }

        [Theory, AutoData]
        public async Task GetOfferingsForCourse_ThrowsforNullCourse(Guid id)
        {
            Courses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetOfferingsForCourseAsync(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
