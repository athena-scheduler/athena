using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Exceptions;
using System.Net;

namespace Athena.Controllers.api
{
   [Route("api/v1/course/{id}/offering")]

    public class OfferingCourseController : Controller
    {
        private readonly IOfferingReository _offerings;
        private readonly ICourseRepository _courses;

        public OfferingCourseController(IOfferingReository offeringsRepository, ICourseRepository coursesRepository)
        {
            _offerings = offeringsRepository ?? throw new ArgumentNullException(nameof(offeringsRepository));
            _courses = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
        }

        [HttpGet]
        public async Task<IEnumerable<Offering>> GetOfferingsForCourseAsync(Course course)
        {
            if (course == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to get offering for {course} that doesn't exist");
            }
            return await _offerings.GetOfferingsForCourseAsync(course);
        }

        [HttpPost("{offeringId}")]
        public async Task AddOfferingAsync(Course course, Offering offering)
        {
            if (offering == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to add offering {offering} to course {course} that doesn't exist");
            }
            await _courses.AddOfferingAsync(course, offering);
        }

        [HttpDelete("{offeringId}")]
        public async Task RemoveOfferingAsync(Course course, Offering offering)
        {
            if (offering == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to remove offering {offering} to course {course} that doesn't exist");
            }
            await _courses.RemoveOfferingAsync(course, offering);
        }
    }
}
