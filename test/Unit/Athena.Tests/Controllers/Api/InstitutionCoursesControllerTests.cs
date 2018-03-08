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
using Athena.Tests.Extensions;

namespace Athena.Tests.Controllers.Api
{
   public class InstitutionCoursesControllerTests : ControllerTest
    {
        private readonly InstitutionCoursesController _controller;

        public InstitutionCoursesControllerTests() => _controller = new InstitutionCoursesController(Institutions.Object, Coureses.Object);

        [Theory, AutoData]
        public async Task GetCoursesForInstitution_Valid(Guid institutionId, Institution institution)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(institution);

            await _controller.GetCoursesForInstitutionAsync(institutionId);

            Coureses.Verify(c => c.GetCoursesForInstitutionAsync(institution));
        }

        [Theory, AutoData]
        public async Task GetCoursesForInstitution_ThrowsforNullInstitution(Guid institutionId)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetCoursesForInstitutionAsync(institutionId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    } 
}
