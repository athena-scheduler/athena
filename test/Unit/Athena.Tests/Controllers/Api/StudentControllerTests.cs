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
    public class StudentControllerTests : ControllerTest
    {
        private readonly StudentController _controller;

        public StudentControllerTests() => _controller = new StudentController(Students.Object, Programs.Object, Institutions.Object);

        [Theory, AutoData]
        public async Task Add_valid(Student student)
        {
            await _controller.AddStudent(student);

            Students.Verify(c => c.AddAsync(student), Times.Once);
        }

        [Theory, AutoData]
        public async Task Get_Valid(Student student)
        {
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);

            var result = await _controller.GetStudent(student.Id);

            Assert.Equal(student, result);
        }

        [Theory, AutoData]
        public async Task Edit_Valid(Student student)
        {
            await _controller.EditStudent(student.Id, student);

            Students.Verify(c => c.EditAsync(student), Times.Once);
        }

        [Theory, AutoData]
        public async Task Edit_ThrowsForMismatchedId(Guid id, Student student)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EditStudent(id, student));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task Delete_Valid(Student student)
        {
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);

            await _controller.DeleteStudent(student.Id);

            Students.Verify(c => c.DeleteAsync(student), Times.Once);
        }

        [Theory, AutoData]
        public async Task Delete_ThrowsforNullStudent(Guid id)
        {
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Student));

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteStudent(id));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetInstitutionsforStudent_Valid(Student student)
        {
            await _controller.GetInstitutionsForStudentAsync(student);

            Institutions.Verify(i => i.GetInstitutionsForStudentAsync(student));
        }

        [Fact]
        public async Task GetInstitutionsforStudent_ThrowsforNullStudent()
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetInstitutionsForStudentAsync(null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task EnrollStudent_Valid(Institution institution, Student student)
        {
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Student));

            await _controller.EnrollStudentAsync(institution, student);

            Institutions.Verify(i => i.GetInstitutionsForStudentAsync(student));
        }

        [Theory, AutoData]
        public async Task EnrollStudent_ThrowsforNullStudent(Institution institution)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EnrollStudentAsync(institution, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task EnrollStudent_ThrowsforNullInstitution(Student student)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EnrollStudentAsync(null, student));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task UnenrollStudent_Valid(Institution institution, Student student)
        {
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Student));

            await _controller.UnenrollStudentAsync(institution, student);

            Institutions.Verify(i => i.GetInstitutionsForStudentAsync(student));
        }

        [Theory, AutoData]
        public async Task UnenrollStudent_ThrowsforNullStudent(Institution institution)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EnrollStudentAsync(institution, null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task UnenrollStudent_ThrowsforNullInstitution(Student student)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EnrollStudentAsync(null, student));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetProgramsForStudent_Valid(Student student)
        {
            await _controller.GetProgramsForStudentAsync(student);

            Programs.Verify(i => i.GetProgramsForStudentAsync(student));
        }

        [Fact]
        public async Task GetProgramsforStudent_ThrowsforNullStudent()
        {
            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetProgramsForStudentAsync(null));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ResponseCode);
        }

    }
}
