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
    public class InstitutionControllerTests : ControllerTest
    {
        private readonly InstitutionController _controller;

        public InstitutionControllerTests() => _controller = new InstitutionController(Institutions.Object, Campuses.Object, Students.Object, Programs.Object);

        [Theory, AutoData]
        public async Task Add_valid(Institution institution)
        {
            await _controller.AddInstitution(institution);

            Institutions.Verify(c => c.AddAsync(institution), Times.Once);
        }

        [Theory, AutoData]
        public async Task Get_Valid(Institution institution)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(institution);

            var result = await _controller.GetInstitution(institution.Id);

            Assert.Equal(institution, result);
        }

        [Theory, AutoData]
        public async Task Edit_Valid(Institution institution)
        {
            await _controller.EditInstitution(institution.Id, institution);

            Institutions.Verify(c => c.EditAsync(institution), Times.Once);
        }

        [Theory, AutoData]
        public async Task Edit_ThrowsForMismatchedId(Guid id, Institution institution)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EditInstitution(id, institution));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task Delete_Valid(Institution institution)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(institution);

            await _controller.DeleteInstitution(institution.Id);

            Institutions.Verify(c => c.DeleteAsync(institution), Times.Once);
        }

        [Theory, AutoData]
        public async Task Delete_ThrowsforNullInstitution(Guid id)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Institution));

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteInstitution(id));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }
    }
}
