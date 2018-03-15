using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/institution/{id}/courses")]
    public class InstitutionCoursesController : AthenaApiController
    {
        private readonly IInstitutionRepository _institutions;
        private readonly ICourseRepository _courses;

        public InstitutionCoursesController(IInstitutionRepository institutionRepository,ICourseRepository courseRepository)
        {
            _institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            _courses = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        }

        [HttpGet]
        public async Task<IEnumerable<Course>> GetCoursesForInstitutionAsync(Guid id)
        {
            var institution = (await _institutions.GetAsync(id)).NotFoundIfNull();
 
            return await _courses.GetCoursesForInstitutionAsync(institution);
        }
    }
}