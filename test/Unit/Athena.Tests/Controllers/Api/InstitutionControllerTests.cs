using Athena.Controllers.api;
using Athena.Core.Models;
using Athena.Exceptions;
using Athena.Tests.Extensions;
using AutoFixture.Xunit2;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Athena.Tests.Controllers.Api
{
    public class InstitutionControllerTests : ControllerTest
    {
        private readonly InstitutionController _controller;

        public InstitutionControllerTests() => _controller = new InstitutionController(Institutions.Object);

        [Theory, AutoData]
        public async Task Add_valid(Institution institution)
        {
            await _controller.AddInstitution(institution);

            Institutions.Verify(c => c.AddAsync(institution), Times.Once);
        }

        [Theory, AutoData]
        public async Task Search_Valid(string q, List<Institution> institutions)
        {
            Institutions.Setup(i => i.SearchAsync(It.IsAny<string>())).ReturnsAsync(institutions);

            var result = (await _controller.Search(q)).ToList();
            
            Assert.Equal(institutions.Count, result.Count);
            Assert.All(institutions, i => Assert.Contains(i, result));
            
            Institutions.Verify(i => i.SearchAsync(q), Times.Once);
        }

        [Fact]
        public async Task Search_BadRequestForShortQuery()
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.Search("a"));
            
            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
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
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteInstitution(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
