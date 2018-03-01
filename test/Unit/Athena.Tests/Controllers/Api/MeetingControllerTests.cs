using Athena.Controllers.api;
using Athena.Core.Models;
using Athena.Exceptions;
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
    public class MeetingControllerTests : ControllerTest
    {
        private readonly MeetingController _controller;

        public MeetingControllerTests() => _controller = new MeetingController(Meetings.Object, Offerings.Object);

        [Theory, AutoData]
        public async Task Add_valid(Meeting meeting)
        {
            await _controller.AddMeeting(meeting);

            Meetings.Verify(c => c.AddAsync(meeting), Times.Once);
        }

        [Theory, AutoData]
        public async Task Get_Valid(Meeting meeting)
        {
            Meetings.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(meeting);

            var result = await _controller.GetMeeting(meeting.Id);

            Assert.Equal(meeting, result);
        }

        [Theory, AutoData]
        public async Task Edit_Valid(Meeting meeting)
        {
            await _controller.EditMeeting(meeting.Id, meeting);

            Meetings.Verify(c => c.EditAsync(meeting), Times.Once);
        }

        [Theory, AutoData]
        public async Task Edit_ThrowsForMismatchedId(Guid id, Meeting meeting)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EditMeeting(id, meeting));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task Delete_Valid(Meeting meeting)
        {
            Meetings.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(meeting);

            await _controller.DeleteMeeting(meeting.Id);

            Meetings.Verify(c => c.DeleteAsync(meeting), Times.Once);
        }

        [Theory, AutoData]
        public async Task Delete_ThrowsforNullMeeting(Guid id)
        {
            Meetings.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Meeting));

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteMeeting(id));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }
    }
}
