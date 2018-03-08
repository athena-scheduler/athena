using Athena.Controllers.api;
using System;
using System.Collections.Generic;
using System.Text;
using Athena.Core.Models;
using Athena.Exceptions;
using AutoFixture.Xunit2;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Athena.Tests.Extensions;

namespace Athena.Tests.Controllers.Api
{
   public class ProgramControllerTests : ControllerTest
    {
        private readonly ProgramController _controller;

        public ProgramControllerTests() => _controller = new ProgramController(Programs.Object, Institutions.Object, Students.Object, Requirements.Object);

        [Theory, AutoData]
        public async Task Add_valid(Program program)
        {
            await _controller.AddProgram(program);

            Programs.Verify(p => p.AddAsync(program), Times.Once);
        }

        [Theory, AutoData]
        public async Task Get_Valid(Program program)
        {
            Programs.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(program);

            var result = await _controller.GetProgram(program.Id);

            Assert.Equal(program, result);
        }

        [Theory, AutoData]
        public async Task Edit_Valid(Program program)
        {
            await _controller.EditProgram(program.Id, program);

            Programs.Verify(c => c.EditAsync(program), Times.Once);
        }

        [Theory, AutoData]
        public async Task Edit_ThrowsForMismatchedId(Guid id, Program program)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EditProgram(id, program));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task Delete_Valid(Program program)
        {
            Programs.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(program);

            await _controller.DeleteProgram(program.Id);

            Programs.Verify(c => c.DeleteAsync(program), Times.Once);
        }

        [Theory, AutoData]
        public async Task Delete_ThrowsforNullProgram(Guid id)
        {
            Programs.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Program));

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteProgram(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetRequirementsForProgram_valid(Program program)
        {
            Programs.Setup(p => p.GetAsync(It.IsAny<Guid>())).ReturnsAsync(program);

            await _controller.GetRequirementsForProgramAsync(program.Id);

            Requirements.Verify(p => p.GetRequirementsForProgramAsync(program), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetRequirementsForProgram_ThrowsforNullProgram(Guid id)
        {
            Programs.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetRequirementsForProgramAsync(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
