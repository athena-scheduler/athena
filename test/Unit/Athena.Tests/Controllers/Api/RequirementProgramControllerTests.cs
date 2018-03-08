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
        public async Task AddRequirement_Valid(Program program, Requirement requirement)
        {
            await _controller.AddRequirementAsync(program, requirement);

            Programs.Verify(c => c.AddRequirementAsync(program, requirement));
        }

        [Theory, AutoData]
        public async Task AddRequirement_ThrowsforNullProgram(Requirement requirement)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddRequirementAsync(null, requirement));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemoveRequirement_Valid(Program program, Requirement requirement)
        {
            await _controller.RemoveRequirementAsync(program, requirement);

            Programs.Verify(c => c.RemoveRequirementAsync(program, requirement));
        }

        [Theory, AutoData]
        public async Task RemoveRequirement_ThrowsforNullProgram(Requirement requirement)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RemoveRequirementAsync(null, requirement));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

    }
}
