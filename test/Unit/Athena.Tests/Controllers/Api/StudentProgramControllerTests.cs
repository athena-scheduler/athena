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
   public class StudentProgramControllerTests : ControllerTest
    {
        private readonly StudentProgramController _controller;

        public StudentProgramControllerTests() => _controller = new StudentProgramController(Students.Object, Programs.Object);

        [Theory, AutoData]
        public async Task RegisterStudentForProgram_Valid(Guid studentId, Guid programId , Program program, Student student)
        {
            Programs.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(program);
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);

            await _controller.RegisterStudentForProgram(studentId, programId);

            Students.Verify(s => s.RegisterForProgramAsync(student, program), Times.Once);
        }

        [Theory, AutoData]
        public async Task RegisterStudentForProgram_ThrowsforNullProgram(Guid studentId , Guid programId)
        {
            Programs.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RegisterStudentForProgram(studentId, programId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RegisterStudentForProgram_ThrowsforNullStudent(Guid studentId, Guid programId)
        {
            Programs.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RegisterStudentForProgram(studentId, programId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task UnRegisterForProgram_Valid(Guid studentId, Guid programId, Program program, Student student)
        {
            Programs.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(program);
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);

            await _controller.UnregisterForProgram(studentId, programId);

            Students.Verify(s => s.UnregisterForProgramAsync(student, program), Times.Once);
        }

        [Theory, AutoData]
        public async Task UnRegisterForProgram_ThrowsforNullProgram(Guid studentId, Guid programId)
        {
            Programs.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.UnregisterForProgram(studentId, programId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task UnRegisterForProgram_ThrowsforNullStudent(Guid studentId, Guid programId)
        {
            Programs.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.UnregisterForProgram(studentId, programId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
