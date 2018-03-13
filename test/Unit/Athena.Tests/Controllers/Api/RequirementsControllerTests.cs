using Athena.Controllers.api;
using System;
using Athena.Core.Models;
using Athena.Exceptions;
using AutoFixture.Xunit2;
using Moq;
using System.Threading.Tasks;
using Xunit;
using System.Net;

namespace Athena.Tests.Controllers.Api
{
    public class RequirementsControllerTests : ControllerTest
    {
        private readonly RequirementController _controller;

        public RequirementsControllerTests() => _controller = new RequirementController(Requirements.Object);

        [Theory, AutoData]
        public async Task Get_Valid(Requirement requirement)
        {
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            var result = await _controller.GetRequirement(requirement.Id);

            Assert.Equal(requirement, result);
        }

        [Theory, AutoData]
        public async Task Edit_Valid(Requirement requirement)
        {
            await _controller.EditRequirement(requirement.Id, requirement);

            Requirements.Verify(c => c.EditAsync(requirement), Times.Once);
        }

        [Theory, AutoData]
        public async Task Edit_ThrowsForMismatchedId(Guid id, Requirement requirement)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EditRequirement(id, requirement));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task Delete_Valid(Requirement requirement)
        {
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            await _controller.DeleteRequirement(requirement.Id);

            Requirements.Verify(c => c.DeleteAsync(requirement), Times.Once);
        }

        [Theory, AutoData]
        public async Task Delete_ThrowsforNullRequirement(Guid id)
        {
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Requirement));

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteRequirement(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
