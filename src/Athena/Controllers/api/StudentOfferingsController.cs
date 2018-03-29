using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Repositories;
using Athena.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers.api
{
    [Route("api/v1/student/{id}/offerings")]
    public class StudentOfferingsController : AthenaApiController
    {
        private readonly IStudentRepository _students;
        private readonly IOfferingReository _offerings;

        public StudentOfferingsController(IStudentRepository students, IOfferingReository offerings)
        {
            _students = students ?? throw new ArgumentNullException(nameof(students));
            _offerings = offerings ?? throw new ArgumentNullException(nameof(offerings));
        }

        [HttpGet]
        public async Task<IEnumerable<Offering>> GetEnrolledOfferings(Guid id)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();

            return await _offerings.GetInProgressOfferingsForStudentAsync(student);
        }

        [HttpPut("{offeringId}")]
        public async Task EnrollInOffering(Guid id, Guid offeringId)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            var offering = (await _offerings.GetAsync(offeringId)).NotFoundIfNull();

            await _offerings.EnrollStudentInOfferingAsync(student, offering);
        }

        [HttpDelete("{offeringId}")]
        public async Task UnenrollInOffering(Guid id, Guid offeringId)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            var offering = (await _offerings.GetAsync(offeringId)).NotFoundIfNull();

            await _offerings.UnenrollStudentInOfferingAsync(student, offering);
        }
    }
}