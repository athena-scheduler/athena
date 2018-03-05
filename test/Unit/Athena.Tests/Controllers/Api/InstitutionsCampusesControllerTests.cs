using Athena.Controllers.api;
using Athena.Core.Models;
using Athena.Exceptions;
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
        public async Task GetCampusesforInstitution_Valid(Institution institution)
        {
            await _controller.GetCampusesForInstitutionAsync(institution);

            Campuses.Verify(c => c.GetCampusesForInstitutionAsync(institution), Times.Once);
        }

        [Fact]
        public async Task GetCampusesforInstitution_ThrowsforNullInstitution()
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetCampusesForInstitutionAsync(null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task AssociateCampusWithInstitution_Valid(Campus campus, Institution institution)
        {
            await _controller.AssociateCampusWithInstitutionAsync(campus, institution);

            Campuses.Verify(c => c.AssociateCampusWithInstitutionAsync(campus, institution));
        }

        [Theory, AutoData]
        public async Task AssassociateCampusWithInstitution_ThrowsforNullInstitution(Campus campus)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.AssociateCampusWithInstitutionAsync(campus, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task DissassociateCampusWithInstitution_Valid(Campus campus, Institution institution)
        {
            await _controller.DissassociateCampusWithInstitutionAsync(campus, institution);

            Campuses.Verify(c => c.DissassociateCampusWithInstitutionAsync(campus, institution));
        }

        [Theory, AutoData]
        public async Task DissassociateCampusWithInstitution_ThrowsforNullInstitution(Campus campus)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DissassociateCampusWithInstitutionAsync(campus, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }
    }
}
