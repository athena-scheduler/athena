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
   public class OfferingMeetingControllerTests : ControllerTest
    {
        private readonly OfferingMeetingController _controller;

        public OfferingMeetingControllerTests() => _controller = new OfferingMeetingController(Offerings.Object, Meetings.Object);

        [Theory, AutoData]
        public async Task AddMeeting_Valid(Guid offeringId, Guid meetingId, Offering offering, Meeting meeting)
        {
            Offerings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(offering);
            Meetings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(meeting);

            await _controller.AddMeetingAsync(offeringId, meetingId);

            Offerings.Verify(o => o.AddMeetingAsync(offering, meeting));
        }

        [Theory, AutoData]
        public async Task AddMeeting_ThrowsforNullMeeting(Guid offeringId , Guid meetingId)
        {
            Offerings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Meetings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddMeetingAsync(offeringId, meetingId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemoveMeeting_Valid(Guid offeringId, Guid meetingId, Offering offering, Meeting meeting)
        {
            Offerings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(offering);
            Meetings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(meeting);

            await _controller.RemoveMeetingAsync(offeringId, meetingId);

            Offerings.Verify(o => o.RemoveMeetingAsync(offering, meeting));
        }

        [Theory, AutoData]
        public async Task RemoveMeeting_ThrowsforNullMeeting(Guid offeringId , Guid meetingId)
        {
            Offerings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Meetings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RemoveMeetingAsync(offeringId, meetingId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetMeetingsForOffering(Guid offeringId , Offering offering)
        {
            Offerings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(offering);

            await _controller.GetMeetingsForOfferingAsync(offeringId);

            Meetings.Verify(o => o.GetMeetingsForOfferingAsync(offering));
        }

        [Theory, AutoData]
        public async Task GetMeetingsForOffering_ThrowsforNullCourse(Guid offeringId)
        {
            Offerings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetMeetingsForOfferingAsync(offeringId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
