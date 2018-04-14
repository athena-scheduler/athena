using Athena.Controllers.api;
using System;
using System.Collections.Generic;
using System.Linq;
using Athena.Core.Models;
using Athena.Exceptions;
using AutoFixture.Xunit2;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Athena.Core.Repositories;
using Xunit;

namespace Athena.Tests.Controllers.Api
{
   public class ProgramControllerTests : ControllerTest
    {
        private readonly ProgramController _controller;

        public ProgramControllerTests() => _controller = new ProgramController(Programs.Object);

        [Theory, AutoData]
        public async Task Add_valid(Program program)
        {
            await _controller.AddProgram(program);

            Programs.Verify(p => p.AddAsync(program), Times.Once);
        }

        [Theory, AutoData]
        public async Task Search_Valid(string q, List<Program> programs)
        {
            Programs.Setup(p => p.SearchAsync(It.IsAny<ProgramSearchOptions>())).ReturnsAsync(programs);

            var result = (await _controller.Search(q, Guid.Empty)).ToList();
            
            Assert.Equal(programs.Count, result.Count);
            Assert.All(programs, p => Assert.Contains(p, result));
            
            Programs.Verify(p => p.SearchAsync(new ProgramSearchOptions{Query = q}), Times.Once);
        }

        [Fact]
        public async Task Search_BadRequestForShortQuery()
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.Search("a", Guid.Empty));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
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
    }
}
