using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Exceptions;
using System.Net;

namespace Athena.Controllers.api
{
    [Route("api/v1/institution/{id}/courses")]
    public class InstitutionCoursesController : Controller
    {
        private readonly IInstitutionRepository _institutions;
        private readonly ICourseRepository _courses;

        public InstitutionCoursesController(IInstitutionRepository institutionRepository,ICourseRepository courseRepository)
        {
            _institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            _courses = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        }

        [HttpGet]
        public async Task<IEnumerable<Course>> GetCoursesForInstitutionAsync(Guid institutionId)
        {
            var institution = await _institutions.GetAsync(institutionId);
            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to get course for {institution} that does not exist");
            }
            return await _courses.GetCoursesForInstitutionAsync(institution);
        }
    }
}