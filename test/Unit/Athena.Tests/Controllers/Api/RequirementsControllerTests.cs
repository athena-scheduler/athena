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
    public class RequirementsControllerTests : ControllerTest
    {
        private readonly RequirementController _controller;

        public RequirementsControllerTests() => _controller = new RequirementController(Requirements.Object, Programs.Object, Coureses.Object);

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

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task AddSatisfiedRequirement_Valid(Course course, Requirement requirement)
        {
            await _controller.AddSatisfiedRequirementAsync(course, requirement);

            Coureses.Verify(c => c.AddSatisfiedRequirementAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task AddSatisfiedRequirement_ThrowsforNullRequirement(Course course)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddSatisfiedRequirementAsync(course, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemoveSatisfiedRequirement_Valid(Course course, Requirement requirement)
        {
            await _controller.RemoveSatisfiedRequirementAsync(course, requirement);

            Coureses.Verify(c => c.RemoveSatisfiedRequirementAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task RemoveSatisfiedRequirement_ThrowsforNullRequirement(Course course)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RemoveSatisfiedRequirementAsync(course, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task AddPrerequisite_Valid(Course course, Requirement requirement)
        {
            await _controller.AddPrerequisiteAsync(course, requirement);

            Coureses.Verify(c => c.AddPrerequisiteAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task AddPrerequisite_ThrowsforNullRequirement(Course course)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddPrerequisiteAsync(course, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemovePrerequisite_Valid(Course course, Requirement requirement)
        {
            await _controller.RemovePrerequisiteAsync(course, requirement);

            Coureses.Verify(c => c.RemovePrerequisiteAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task RemovePrerequisite_ThrowsforNullRequirement(Course course)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RemovePrerequisiteAsync(course, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task AddConcurrentPrerequisiteAsync_Valid(Course course, Requirement requirement)
        {
            await _controller.AddConcurrentPrerequisiteAsync(course, requirement);

            Coureses.Verify(c => c.AddConcurrentPrerequisiteAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task AddConcurrentPrerequisiteAsync_ThrowsforNullPrereq(Course course)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddConcurrentPrerequisiteAsync(course, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemoveConcurrentPrerequisiteAsync_Valid(Course course, Requirement requirement)
        {
            await _controller.RemoveConcurrentPrerequisiteAsync(course, requirement);

            Coureses.Verify(c => c.RemoveConcurrentPrerequisiteAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task RemoveConcurrentPrerequisiteAsync_ThrowsforNullPrereq(Course course)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RemoveConcurrentPrerequisiteAsync(course, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task AddRequirement_Valid(Program program , Requirement requirement)
        {
            await _controller.AddRequirementAsync(program, requirement);

            Programs.Verify(c => c.AddRequirementAsync(program, requirement));
        }

        [Theory, AutoData]
        public async Task AddRequirement_ThrowsforNullProgram(Requirement requirement)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddRequirementAsync(null, requirement));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
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

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }
    }
}
