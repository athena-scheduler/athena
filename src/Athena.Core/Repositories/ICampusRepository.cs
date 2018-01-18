using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface ICampusRepository : IRepository<Campus, Guid>
    {
        /// <summary>
        /// Get the campuses on which the specified institution offers courses
        /// </summary>
        /// <param name="institution">The institution to get campuses for</param>
        /// <returns>An IEnumerable of Cmpuses</returns>
        Task<IEnumerable<Campus>> GetCampusesForInstitutionAsync(Institution institution);
    }
}