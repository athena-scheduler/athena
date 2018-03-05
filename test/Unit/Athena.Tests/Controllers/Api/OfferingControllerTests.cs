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

namespace Athena.Tests.Controllers.Api
{
    public class OfferingControllerTests : ControllerTest
    {
        private readonly OfferingController _controller;

        public OfferingControllerTests() => _controller = new OfferingController(Offerings.Object, Meetings.Object, Coureses.Object);

        [Theory, AutoData]
        public async Task Add_valid(Offering offering)
        {
            await _controller.AddOffering(offering);

            Offerings.Verify(c => c.AddAsync(offering), Times.Once);
        }

        [Theory, AutoData]
        public async Task Get_Valid(Offering offering)
        {
            Offerings.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(offering);

            var result = await _controller.GetOffering(offering.Id);

            Assert.Equal(offering, result);
        }

        [Theory, AutoData]
        public async Task Edit_Valid(Offering offering)
        {
            await _controller.EditOffering(offering.Id, offering);

            Offerings.Verify(c => c.EditAsync(offering), Times.Once);
        }

        [Theory, AutoData]
        public async Task Edit_ThrowsForMismatchedId(Guid id, Offering offering)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EditOffering(id, offering));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task Delete_Valid(Offering offering)
        {
            Offerings.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(offering);

            await _controller.DeleteOffering(offering.Id);

            Offerings.Verify(c => c.DeleteAsync(offering), Times.Once);
        }

        [Theory, AutoData]
        public async Task Delete_ThrowsforNullOffering(Guid id)
        {
            Offerings.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Offering));

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteOffering(id));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

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

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task AddMeeting_Valid(Offering offering, Meeting meeting)
        {
            await _controller.AddMeetingAsync(offering, meeting);

            Offerings.Verify(o => o.AddMeetingAsync(offering, meeting));
        }

        [Theory, AutoData]
        public async Task AddMeeting_ThrowsforNullMeeting(Offering offering)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddMeetingAsync(offering, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemoveMeeting_Valid(Offering offering, Meeting meeting)
        {
            await _controller.RemoveMeetingAsync(offering, meeting);

            Offerings.Verify(o => o.RemoveMeetingAsync(offering, meeting));
        }

        [Theory, AutoData]
        public async Task RemoveMeeting_ThrowsforNullMeeting(Offering offering)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RemoveMeetingAsync(offering, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetMeetingsForOffering(Offering offering)
        {
            await _controller.GetMeetingsForOfferingAsync(offering);

            Meetings.Verify(o => o.GetMeetingsForOfferingAsync(offering));
        }

        [Fact]
        public async Task GetMeetingsForOffering_ThrowsforNullCourse()
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetMeetingsForOfferingAsync(null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task AddOffering_Valid(Course course, Offering offering)
        {
            await _controller.AddOfferingAsync(course, offering);

            Coureses.Verify(o => o.AddOfferingAsync(course, offering));
        }

        [Theory, AutoData]
        public async Task AddOffering_ThrowsforNullOffeirng(Course course)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddOfferingAsync(course, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemoveOffering_Valid(Course course, Offering offering)
        {
            await _controller.RemoveOfferingAsync(course, offering);

            Coureses.Verify(o => o.RemoveOfferingAsync(course, offering));
        }

        [Theory, AutoData]
        public async Task RemoveOffering_ThrowsforNullMeeting(Course course)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RemoveOfferingAsync(course, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }
    }
}
