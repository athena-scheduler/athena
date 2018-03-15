using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Repositories;
using Athena.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers.api
{
    public class StudentInstitutionController : AthenaApiController
    {
        private readonly IStudentRepository _students;
        private readonly IInstitutionRepository _institutions;
        
        public StudentInstitutionController(IStudentRepository studentsRepository, IInstitutionRepository institutionsRepository)
        {
            _students = studentsRepository ?? throw new ArgumentNullException(nameof(studentsRepository));
            _institutions = institutionsRepository ?? throw new ArgumentNullException(nameof(institutionsRepository));
        }
        
        [HttpGet("{id}/institutions")]
        public async Task<IEnumerable<Institution>> GetInstitutionsForStudentAsync(Guid id)
        {
            var student = (await _students.GetAsync(id)).NotFoundIfNull();
            
            return await _institutions.GetInstitutionsForStudentAsync(student);
        }

        [HttpPost("{id}/institutions/{institutionId}")]
        public async Task EnrollStudentAsync(Guid institutionId, Guid studentId)
        {
            var institution = (await _institutions.GetAsync(institutionId)).NotFoundIfNull();
            var student = (await _students.GetAsync(studentId)).NotFoundIfNull();

            await _institutions.EnrollStudentAsync(institution, student);
        }

        [HttpDelete("{id}/institutions/{institutionId}")]
        public async Task UnenrollStudentAsync(Guid institutionId, Guid studentId)
        {
            var institution = (await _institutions.GetAsync(institutionId)).NotFoundIfNull();
            var student = (await _students.GetAsync(studentId)).NotFoundIfNull();

            await _institutions.UnenrollStudentAsync(institution, student);
        }
    }
}