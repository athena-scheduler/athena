using System;
using System.Net;
using System.Threading.Tasks;
using Athena.Controllers.api;
using Athena.Core.Models;
using Athena.Exceptions;
using Athena.Tests.Extensions;
using AutoFixture.Xunit2;
using Moq;
using Xunit;

namespace Athena.Tests.Controllers.Api
{
    public class InstitutionProgramControllerTests : ControllerTest
    {
        private readonly InstitutionProgramController _controller;

        public InstitutionProgramControllerTests() =>
            _controller = new InstitutionProgramController(Institutions.Object, Programs.Object);
        
        [Theory, AutoData]
        public async Task GetProgramsOfferedByInstitution(Guid institutionId, Institution institution)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(institution);

            await _controller.GetProgramsOfferedByInstitutionAsync(institutionId);

            Programs.Verify(p => p.GetProgramsOfferedByInstitutionAsync(institution), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetProgramsOfferedByInstitution_ThrowsforNullInstitution(Guid institionId)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetProgramsOfferedByInstitutionAsync(institionId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}