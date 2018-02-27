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
    public class CampusControllerTests : ControllerTest
    {
        private readonly CampusController _controller;

        public CampusControllerTests() => _controller = new CampusController(Campuses.Object, Institutions.Object);

        [Theory, AutoData]
        public async Task Add_valid(Campus campus)
        {
            await _controller.AddCampus(campus);

            Campuses.Verify(c => c.AddAsync(campus), Times.Once);
        }

        [Theory, AutoData]
        public async Task Get_Valid(Campus campus)
        {
            Campuses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(campus);

            var result = await _controller.GetCampus(campus.Id);

            Assert.Equal(campus, result);
        }

        [Theory, AutoData]
        public async Task Edit_Valid(Campus campus)
        {
            await _controller.EditCampus(campus.Id, campus);

            Campuses.Verify(c => c.EditAsync(campus), Times.Once);
        }

        [Theory, AutoData]
        public async Task Edit_ThrowsForMismatchedId(Guid id, Campus campus)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EditCampus(id, campus));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task Delete_Valid (Campus campus)
        {
            Campuses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(campus);

            await _controller.DeleteCampus(campus.Id);

            Campuses.Verify(c => c.DeleteAsync(campus), Times.Once);
        }

        [Theory, AutoData]
        public async Task Delete_ThrowsforNullCampus(Guid id)
        {
            Campuses.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Campus));

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteCampus(id));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
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

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

    }
}
