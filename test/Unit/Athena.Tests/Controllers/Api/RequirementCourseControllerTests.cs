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
        public async Task AddPrerequisite_Valid(Course course, Requirement requirement)
        {
            await _controller.AddPrerequisiteAsync(course, requirement);

            Coureses.Verify(c => c.AddPrerequisiteAsync(course, requirement));
        }

        [Theory, AutoData]
        public async Task AddPrerequisite_ThrowsforNullRequirement(Course course)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AddPrerequisiteAsync(course, null));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
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

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
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

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
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

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
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

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
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

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
