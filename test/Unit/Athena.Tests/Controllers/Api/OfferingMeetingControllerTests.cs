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
   public class OfferingMeetingControllerTests : ControllerTest
    {
        private readonly OfferingMeetingController _controller;

        public OfferingMeetingControllerTests() => _controller = new OfferingMeetingController(Offerings.Object, Meetings.Object);

        [Theory, AutoData]
        public async Task GetMeetingsForOffering(Guid offeringId , Offering offering)
        {
            Offerings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(offering);

            await _controller.GetMeetingsForOfferingAsync(offeringId);

            Meetings.Verify(o => o.GetMeetingsForOfferingAsync(offering));
        }

        [Theory, AutoData]
        public async Task GetMeetingsForOffering_ThrowsforNullOffering(Guid offeringId)
        {
            Offerings.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetMeetingsForOfferingAsync(offeringId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
