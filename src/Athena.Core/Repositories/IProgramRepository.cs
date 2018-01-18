using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IProgramRepository : IRepository<Program, Guid>
    {
        /// <summary>
        /// Get the programs that the specified institution offers
        /// </summary>
        /// <param name="institution">The institution to get programs for</param>
        /// <returns>An IEnumerable of Programs</returns>
        Task<IEnumerable<Program>> GetProgramsOfferedByInstitutionAsync(Institution institution);

        /// <summary>
        /// Get the programs that the specified student is enrolled in
        /// </summary>
        /// <param name="student">The student to get programs for</param>
        /// <returns>An IEnumerable of Programs</returns>
        Task<IEnumerable<Program>> GetProgramsForStudentAsync(Student student);
    }
}