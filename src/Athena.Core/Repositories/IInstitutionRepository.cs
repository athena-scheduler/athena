using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IInstitutionRepository : IRepository<Institution, Guid>
    {
        /// <summary>
        /// Gets all instutions that offer courses on the specified campus
        /// </summary>
        /// <param name="campus">The campus to get institutions for</param>
        /// <returns>An IEnumerable of Institutions</returns>
        Task<IEnumerable<Institution>> GetInstitutionsOnCampusAsync(Campus campus);

        /// <summary>
        /// Gets all institutions that the student is interested in taking courses from
        /// </summary>
        /// <param name="student">The student to get institutions for</param>
        /// <returns>An IEnumerable of Institutions</returns>
        Task<IEnumerable<Institution>> GetInstitutionsForStudentAsync(Student student);
    }
}