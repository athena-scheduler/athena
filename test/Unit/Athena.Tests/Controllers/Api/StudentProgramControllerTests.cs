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
        public async Task RegisterStudentForProgram_Valid(Program program, Student student)
        {
            await _controller.RegisterStudentForProgram(student, program);

            Students.Verify(s => s.RegisterForProgramAsync(student, program), Times.Once);
        }

        [Theory, AutoData]
        public async Task RegisterStudentForProgram_ThrowsforNullProgram(Student student)
        {

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RegisterStudentForProgram(student, null));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task RegisterStudentForProgram_ThrowsforNullStudent(Program program)
        {

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.RegisterStudentForProgram(null, program));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task UnRegisterForProgram_Valid(Program program, Student student)
        {
            await _controller.UnregisterForProgram(student, program);

            Students.Verify(s => s.UnregisterForProgramAsync(student, program), Times.Once);
        }

        [Theory, AutoData]
        public async Task UnRegisterForProgram_ThrowsforNullProgram(Student student)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.UnregisterForProgram(student, null));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task UnRegisterForProgram_ThrowsforNullStudent(Program program)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.UnregisterForProgram(null, program));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}
