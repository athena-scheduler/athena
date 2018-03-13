using Athena.Controllers.api;
using Athena.Core.Models;
using Athena.Exceptions;
using Athena.Tests.Extensions;
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
   public class RequirementProgramControllerTests : ControllerTest
    {
        private readonly RequirementProgramController _controller;

        public RequirementProgramControllerTests() => _controller = new RequirementProgramController(Requirements.Object, Programs.Object);
        [Theory, AutoData]
        public async Task AddRequirement_Valid(Guid programId, Program program, Requirement requirement)
        {
            Programs.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(program);
            Requirements.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            await _controller.AddRequirementAsync(programId, requirement);

            Programs.Verify(c => c.AddRequirementAsync(program, requirement));
        }

        [Theory, AutoData]
        public async Task AddRequirement_ThrowsforNullProgram(Guid programId, Program program, Requirement requirement)
        {
            Programs.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Requirements.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddRequirementAsync(programId, requirement));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemoveRequirement_Valid(Guid programId , Guid requirementId, Program program, Requirement requirement)
        {
            Programs.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(program);
            Requirements.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            await _controller.RemoveRequirementAsync(programId, requirementId);

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

    }
}
