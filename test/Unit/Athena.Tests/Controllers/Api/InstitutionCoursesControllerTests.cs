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

namespace Athena.Tests.Controllers.Api
{
   public class InstitutionCoursesControllerTests : ControllerTest
    {
        private readonly InstitutionCoursesController _controller;

        public InstitutionCoursesControllerTests() => _controller = new InstitutionCoursesController(Institutions.Object, Coureses.Object);

        [Theory, AutoData]
        public async Task GetCoursesForInstitution_Valid(Institution institution)
        {
            await _controller.GetCoursesForInstitutionAsync(institution);

            Coureses.Verify(c => c.GetCoursesForInstitutionAsync(institution));
        }

        [Fact]
        public async Task GetCoursesForInstitution_ThrowsforNullInstitution()
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetCoursesForInstitutionAsync(null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }
    } 
}
