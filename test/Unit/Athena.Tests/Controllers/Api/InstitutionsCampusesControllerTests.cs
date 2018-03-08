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
    public class InstitutionsCampusesControllerTests : ControllerTest
    {
        private readonly InstitutionCampusesController _controller;

        public InstitutionsCampusesControllerTests() => _controller = new InstitutionCampusesController(Campuses.Object, Institutions.Object);

        [Theory, AutoData]
        public async Task GetCampusesforInstitution_Valid(Guid institutionId, Institution institution)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(institution);

            await _controller.GetCampusesForInstitutionAsync(institutionId);

            Campuses.Verify(c => c.GetCampusesForInstitutionAsync(institution), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetCampusesforInstitution_ThrowsforNullInstitution(Guid institutionId)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetCampusesForInstitutionAsync(institutionId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task AssociateCampusWithInstitution_Valid(Guid campusId, Guid institutionId, Institution institution, Campus campus)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(institution);
            Campuses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(campus);

            await _controller.AssociateCampusWithInstitutionAsync(campusId, institutionId);

            Campuses.Verify(c => c.AssociateCampusWithInstitutionAsync(campus, institution));
        }

        [Theory, AutoData]
        public async Task AssassociateCampusWithInstitution_ThrowsforNullInstitution(Guid campusId, Guid institutionId)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AssociateCampusWithInstitutionAsync(campusId, institutionId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task DissassociateCampusWithInstitution_Valid(Guid campusId, Guid institutionId, Institution institution, Campus campus)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(institution);
            Campuses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(campus);

            await _controller.DissassociateCampusWithInstitutionAsync(campusId, institutionId);

            Campuses.Verify(c => c.DissassociateCampusWithInstitutionAsync(campus, institution));
        }

        [Theory, AutoData]
        public async Task DissassociateCampusWithInstitution_ThrowsforNullInstitution(Guid campusId, Guid institutionId)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DissassociateCampusWithInstitutionAsync(campusId, institutionId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetInstitutionOnCampus_Valid(Campus campus)
        {
            await _controller.GetInstitutionsOnCampusAsync(campus);

            Institutions.Verify(c => c.GetInstitutionsOnCampusAsync(campus), Times.Once);
        }

        [Fact]
        public async Task GetInstitutionOnCampus_ThrowsforNullCampus()
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetInstitutionsOnCampusAsync(null));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

    }
}
