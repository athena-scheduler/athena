﻿using System;
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
    }
}
