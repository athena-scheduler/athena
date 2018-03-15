using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/course/{id}/offering")]
    public class OfferingCourseController : AthenaApiController
    {
        private readonly IOfferingReository _offerings;
        private readonly ICourseRepository _courses;

        public OfferingCourseController(IOfferingReository offeringsRepository, ICourseRepository coursesRepository)
        {
            _offerings = offeringsRepository ?? throw new ArgumentNullException(nameof(offeringsRepository));
            _courses = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
        }

        [HttpGet]
        public async Task<IEnumerable<Offering>> GetOfferingsForCourseAsync(Guid id)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            
            return await _offerings.GetOfferingsForCourseAsync(course);
        }

        [HttpPut("{offeringId}")]
        public async Task AddOfferingAsync(Guid id, Guid offeringId)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            var offering = (await _offerings.GetAsync(offeringId)).NotFoundIfNull();

            await _courses.AddOfferingAsync(course, offering);
        }

        [HttpDelete("{offeringId}")]
        public async Task RemoveOfferingAsync(Guid id, Guid offeringId)
        {
            var course = (await _courses.GetAsync(id)).NotFoundIfNull();
            var offering = (await _offerings.GetAsync(offeringId)).NotFoundIfNull();

            await _courses.RemoveOfferingAsync(course, offering);
        }
    }
}
