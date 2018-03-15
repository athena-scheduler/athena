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
    public class OfferingControllerTests : ControllerTest
    {
        private readonly OfferingController _controller;

        public OfferingControllerTests() => _controller = new OfferingController(Offerings.Object);

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
            Offerings.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteOffering(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

    }
}
