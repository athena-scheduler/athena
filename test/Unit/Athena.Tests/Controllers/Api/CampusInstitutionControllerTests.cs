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
    public class CampusInstitutionControllerTests : ControllerTest
    {
        private readonly CampusInstitutionController _controller;

        public CampusInstitutionControllerTests() =>
            _controller = new CampusInstitutionController(Campuses.Object, Institutions.Object);
        
        [Theory, AutoData]
        public async Task GetInstitutionOnCampus_Valid(Guid campusId, Campus campus)
        {
            Campuses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(campus);

            await _controller.GetInstitutionsOnCampusAsync(campusId);

            Institutions.Verify(c => c.GetInstitutionsOnCampusAsync(campus), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetInstitutionOnCampus_ThrowsforNullCampus(Guid campusId)
        {
            Campuses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetInstitutionsOnCampusAsync(campusId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}