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
   public class RequirementCourseControllerTests : ControllerTest
    {
        private readonly RequirementCourseController _controller;

        public RequirementCourseControllerTests() => _controller = new RequirementCourseController(Requirements.Object, Coureses.Object);

        [Theory, AutoData]
        public async Task AddPrerequisite_Valid(Guid courseId , Guid requirementId , Course course, Requirement requirement)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            await _controller.AddPrerequisiteAsync(courseId, requirementId);

            Coureses.Verify(c => c.AddPrerequisiteAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task AddPrerequisite_ThrowsforNullRequirement(Guid courseId, Guid requirementId)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddPrerequisiteAsync(courseId, requirementId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemovePrerequisite_Valid(Guid courseId, Guid requirementId, Course course, Requirement requirement)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            await _controller.RemovePrerequisiteAsync(courseId, requirementId);

            Coureses.Verify(c => c.RemovePrerequisiteAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task RemovePrerequisite_ThrowsforNullRequirement(Guid courseId, Guid requirementId)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RemovePrerequisiteAsync(courseId, requirementId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task AddConcurrentPrerequisiteAsync_Valid(Guid courseId, Guid requirementId, Course course, Requirement requirement)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            await _controller.AddConcurrentPrerequisiteAsync(courseId, requirementId);

            Coureses.Verify(c => c.AddConcurrentPrerequisiteAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task AddConcurrentPrerequisiteAsync_ThrowsforNullPrereq(Guid courseId, Guid requirementId)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddConcurrentPrerequisiteAsync(courseId, requirementId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemoveConcurrentPrerequisiteAsync_Valid(Guid courseId, Guid requirementId, Course course, Requirement requirement)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            await _controller.RemoveConcurrentPrerequisiteAsync(courseId, requirementId);

            Coureses.Verify(c => c.RemoveConcurrentPrerequisiteAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task RemoveConcurrentPrerequisiteAsync_ThrowsforNullPrereq(Guid courseId, Guid requirementId)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RemoveConcurrentPrerequisiteAsync(courseId, requirementId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task AddSatisfiedRequirement_Valid(Guid courseId, Guid requirementId, Course course, Requirement requirement)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            await _controller.AddSatisfiedRequirementAsync(courseId, requirementId);

            Coureses.Verify(c => c.AddSatisfiedRequirementAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task AddSatisfiedRequirement_ThrowsforNullRequirement(Guid courseId, Guid requirementId)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddSatisfiedRequirementAsync(courseId, requirementId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RemoveSatisfiedRequirement_Valid(Guid courseId, Guid requirementId, Course course, Requirement requirement)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(course);
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(requirement);

            await _controller.RemoveSatisfiedRequirementAsync(courseId, requirementId);

            Coureses.Verify(c => c.RemoveSatisfiedRequirementAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task RemoveSatisfiedRequirement_ThrowsforNullRequirement(Guid courseId, Guid requirementId)
        {
            Coureses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Requirements.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RemoveSatisfiedRequirementAsync(courseId, requirementId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
