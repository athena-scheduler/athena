using Athena.Controllers.api;
using Athena.Core.Models;
using Athena.Exceptions;
using Athena.Tests.Extensions;
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
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.DeleteStudent(id));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetInstitutionsforStudent_Valid(Guid studentId, Student student)
        {
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);

            await _controller.GetInstitutionsForStudentAsync(studentId);

            Institutions.Verify(i => i.GetInstitutionsForStudentAsync(student));
        }

        [Theory, AutoData]
        public async Task GetInstitutionsforStudent_ThrowsforNullStudent(Guid studentId)
        {
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetInstitutionsForStudentAsync(studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task EnrollStudent_Valid(Guid institutionId, Guid studentId, Institution institution, Student student)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(institution);
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);

            await _controller.EnrollStudentAsync(institutionId, studentId);

            Institutions.Verify(i => i.EnrollStudentAsync(institution, student));
        }

        [Theory, AutoData]
        public async Task EnrollStudent_ThrowsforNullStudent(Guid institutionId, Guid studentId)
        {
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();


            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EnrollStudentAsync(institutionId, studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task EnrollStudent_ThrowsforNullInstitution(Guid institutionId, Guid studentId)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.EnrollStudentAsync(institutionId, studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task UnenrollStudent_Valid(Guid institutionId, Guid studentId, Institution institution, Student student)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(institution);
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);

            await _controller.UnenrollStudentAsync(institutionId, studentId);

            Institutions.Verify(i => i.UnenrollStudentAsync(institution, student));
        }

        [Theory, AutoData]
        public async Task UnenrollStudent_ThrowsforNullStudent(Guid institutionId, Guid studentId)
        {
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.UnenrollStudentAsync(institutionId, studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task UnenrollStudent_ThrowsforNullInstitution(Guid institutionId, Guid studentId)
        {
            Institutions.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.UnenrollStudentAsync(institutionId, studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

        [Theory, AutoData]
        public async Task GetProgramsForStudent_Valid(Guid studentId , Student student)
        {
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);

            await _controller.GetProgramsForStudentAsync(studentId);

            Programs.Verify(i => i.GetProgramsForStudentAsync(student));
        }

        [Theory, AutoData]
        public async Task GetProgramsforStudent_ThrowsforNullStudent(Guid studentId)
        {
            Students.Setup(c => c.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _controller.GetProgramsForStudentAsync(studentId));

            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }

    }
}
