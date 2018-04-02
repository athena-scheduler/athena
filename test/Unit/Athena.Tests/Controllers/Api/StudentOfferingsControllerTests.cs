using System;
using System.Collections.Generic;
using System.Linq;
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
    public class StudentOfferingsControllerTests : ControllerTest
    {
        private readonly StudentOfferingsController _sut;

        public StudentOfferingsControllerTests() =>
            _sut = new StudentOfferingsController(Students.Object, Offerings.Object, Institutions.Object, Courses.Object);

        [Theory, AutoData]
        public async Task GetEnrolledOfferingsValid(List<Offering> offerings, Student student)
        {
            Students.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Offerings.Setup(o => o.GetInProgressOfferingsForStudentAsync(It.IsAny<Student>())).ReturnsAsync(offerings);

            var result = (await _sut.GetEnrolledOfferings(student.Id)).ToList();
            
            Assert.Equal(offerings.Count, result.Count);
            Assert.All(offerings, o => Assert.Contains(o, result));
        }
        
        [Theory, AutoData]
        public async Task EnrollValid(Student student, Offering offering)
        {
            Students.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Offerings.Setup(o => o.GetAsync(It.IsAny<Guid>())).ReturnsAsync(offering);

            await _sut.EnrollInOffering(student.Id, offering.Id);
            
            Students.Verify(s => s.GetAsync(student.Id), Times.Once);
            Offerings.Verify(o => o.GetAsync(offering.Id), Times.Once);
            
            Offerings.Verify(o => o.EnrollStudentInOfferingAsync(student, offering), Times.Once);
        }

        [Theory, AutoData]
        public async Task Enroll_ThrowsForStudentNotFound(Guid student, Guid offering)
        {
            Students.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _sut.EnrollInOffering(student, offering));
            
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
            Offerings.Verify(o => o.GetAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Theory, AutoData]
        public async Task Enroll_ThrowsForOfferingNotFound(Student student, Guid offering)
        {
            Students.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Offerings.Setup(o => o.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () =>
                await _sut.EnrollInOffering(student.Id, offering));
            
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
        
        [Theory, AutoData]
        public async Task UnenrollValid(Student student, Offering offering)
        {
            Students.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Offerings.Setup(o => o.GetAsync(It.IsAny<Guid>())).ReturnsAsync(offering);

            await _sut.UnenrollInOffering(student.Id, offering.Id);
            
            Students.Verify(s => s.GetAsync(student.Id), Times.Once);
            Offerings.Verify(o => o.GetAsync(offering.Id), Times.Once);
            
            Offerings.Verify(o => o.UnenrollStudentInOfferingAsync(student, offering), Times.Once);
        }

        [Theory, AutoData]
        public async Task Unenroll_ThrowsForStudentNotFound(Guid student, Guid offering)
        {
            Students.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () => await _sut.UnenrollInOffering(student, offering));
            
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
            Offerings.Verify(o => o.GetAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Theory, AutoData]
        public async Task Unenroll_ThrowsForOfferingNotFound(Student student, Guid offering)
        {
            Students.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(student);
            Offerings.Setup(o => o.GetAsync(It.IsAny<Guid>())).ReturnsNullAsync();

            var ex = await Assert.ThrowsAsync<ApiException>(async () =>
                await _sut.UnenrollInOffering(student.Id, offering));
            
            Assert.Equal(HttpStatusCode.NotFound, ex.ResponseCode);
        }
    }
}