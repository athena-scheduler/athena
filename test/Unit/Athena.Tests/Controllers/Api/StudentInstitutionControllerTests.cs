using System;
using System.Net;
using System.Threading.Tasks;
using Athena.Controllers.api;
using Athena.Core.Models;
using Athena.Exceptions;
using Athena.Tests.Extensions;
using AutoFixture.Xunit2;
using Moq;
using Xunit;

namespace Athena.Tests.Controllers.Api
{
    public class StudentInstitutionControllerTests : ControllerTest
    {
        private readonly StudentInstitutionController _controller;

        public StudentInstitutionControllerTests() =>
            _controller = new StudentInstitutionController(Students.Object, Institutions.Object);
        
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
    }
}