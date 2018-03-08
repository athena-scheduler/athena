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

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
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

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
