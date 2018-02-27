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
    public class InstitutionCoursesController : Controller
    {
        private readonly IInstitutionRepository institutions;
        private readonly ICourseRepository courses;

        public InstitutionCoursesController(IInstitutionRepository institutionRepository,ICourseRepository courseRepository)
        {
            institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            courses = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        }

        [HttpGet("api/v1/institution/{id}/courses")]
        public async Task<IEnumerable<Course>> GetCoursesForInstitutionAsync(Institution institution)
        {
            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get course for {institution} that does not exist");
            }
            return (await courses.GetCoursesForInstitutionAsync(institution));
        }
    }
}