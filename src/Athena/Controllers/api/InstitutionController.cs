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
    [Route("api/v1/[Controller]")]
    public class InstitutionController : Controller
    {
        private readonly IInstitutionRepository institutions;
        private readonly ICampusRepository campuses;
        private readonly IStudentRepository students;

        public InstitutionController(IInstitutionRepository institutionRepository, ICampusRepository campusRepository, IStudentRepository studentRepository)
        {
            institutions = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            campuses = campusRepository ?? throw new ArgumentNullException(nameof(campusRepository));
            students = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }

        [HttpPost]
        public async Task AddInstitution([FromBody] Institution institution) => await institutions.AddAsync(institution);

        [HttpGet("{id}")]
        public async Task<Institution> GetInstitution(Guid id) => await institutions.GetAsync(id);

        [HttpPut("{id}")]
        public async Task EditInstitution(Guid id, [FromBody] Institution institution)
        {
            if (id != institution.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {institution.Id}");
            }
            await institutions.EditAsync(institution);
        }

        [HttpDelete("{id}")]
        public async Task DeleteInstitution(Guid id)
        {
            var institution = await institutions.GetAsync(id);
            if (institution == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to delete {institution} that does not exist");
            }
            await institutions.DeleteAsync(institution);
        }

        public async Task<IEnumerable<Institution>> GetInstitutionsOnCampusAsync(Campus campus)
        {
            if (campus == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Institutions for {campus} that does not exist");
            }
            return (await institutions.GetInstitutionsOnCampusAsync(campus));
        }

        public async Task<IEnumerable<Institution>> GetInstitutionsForStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Institutions for {student} that does not exist");
            }
            return (await institutions.GetInstitutionsForStudentAsync(student));
        }

        public async Task EnrollStudentAsync(Institution institution, Student student)
        {
            if (student == null || institution == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Enroll {student} in Institution {institution} where eihter the student or institution not exist");
            }
            await institutions.EnrollStudentAsync(institution, student);
        }

        public async Task UnenrollStudentAsync(Institution institution, Student student)
        {
            if (institution == null || student == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get Unenroll {student} in Institution {institution} where eihter the student or institution not exist");
            }
            await institutions.UnenrollStudentAsync(institution, student);
        }
    }
}