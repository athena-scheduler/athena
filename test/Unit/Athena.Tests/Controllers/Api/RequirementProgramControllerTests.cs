using Athena.Controllers.api;
using Athena.Core.Models;
using Athena.Exceptions;
using Athena.Tests.Extensions;
using AutoFixture.Xunit2;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Athena.Tests.Controllers.Api
{
   public class RequirementProgramControllerTests : ControllerTest
    {
        private readonly RequirementProgramController _controller;

        public RequirementProgramControllerTests() => _controller = new RequirementProgramController(Requirements.Object, Programs.Object);
        [Theory, AutoData]
        public async Task AddRequirement_Valid(Program program, Requirement requirement)
        {
            Programs.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(program);
            Requirements.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            await _controller.AddRequirementAsync(program.Id, requirement.Id);

            Programs.Verify(c => c.AddRequirementAsync(program, requirement));
        }

        [Theory, AutoData]
        public async Task AddRequirement_ThrowsforNullProgram(Program program, Requirement requirement)
        {
            Programs.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Requirements.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddRequirementAsync(program.Id, requirement.Id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemoveRequirement_Valid(Program program, Requirement requirement)
        {
            Programs.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(program);
            Requirements.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            await _controller.RemoveRequirementAsync(program.Id, requirement.Id);

            Programs.Verify(c => c.RemoveRequirementAsync(program, requirement));
        }

        [Theory, AutoData]
        public async Task RemoveRequirement_ThrowsforNullProgram(Guid programId, Guid requirementId)
        {
            Programs.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Requirements.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RemoveRequirementAsync(programId, requirementId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetRequirementsForProgram_valid(Program program)
        {
            Programs.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(program);

            await _controller.GetRequirementsForProgramAsync(program.Id);

            Requirements.Verify(p => p.GetRequirementsForProgramAsync(program), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetRequirementsForProgram_ThrowsforNullProgram(Guid id)
        {
            Programs.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetRequirementsForProgramAsync(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
