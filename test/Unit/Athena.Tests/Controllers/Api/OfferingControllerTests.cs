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

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
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

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
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

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
