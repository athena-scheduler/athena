using Athena.Controllers.api;
using System;
using System.Collections.Generic;
using System.Text;
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

        public OfferingCourseControllerTests() => _controller = new OfferingCourseController(Offerings.Object, Coureses.Object);


        [Theory, AutoData]
        public async Task GetOfferingsForCourse(Course course)
        {
            await _controller.GetOfferingsForCourseAsync(course);

            Offerings.Verify(o => o.GetOfferingsForCourseAsync(course));
        }

        [Fact]
        public async Task GetOfferingsForCourse_ThrowsforNullCourse()
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetOfferingsForCourseAsync(null));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
