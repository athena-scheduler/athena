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

        /// <summary>
        /// Associate the provided campus with the provided institution
        /// </summary>
        /// <param name="campus">The campus the institution now offers courses on</param>
        /// <param name="institution">The institution to associate the campus with</param>
        Task AssociateCampusWithInstitutionAsync(Campus campus, Institution institution);
        
        /// <summary>
        /// Dissassociate the provided campus with the provided institution
        /// </summary>
        /// <param name="campus">The campus the institution no longer offers courses on</param>
        /// <param name="institution">The institutiton to dissassociate the campus with</param>
        Task DissassociateCampusWithInstitutionAsync(Campus campus, Institution institution);

        /// <summary>
        /// Add a room to the campus
        /// </summary>
        /// <param name="campus">The campus to modify</param>
        /// <param name="room">The roomm to add</param>
        Task AddRoomAsync(Campus campus, Room room);
        
        /// <summary>
        /// Remove a room from the campus
        /// </summary>
        /// <param name="campus">The campus to modify</param>
        /// <param name="room">The roomm to remove</param>
        Task RemoveRoomAsync(Campus campus, Room room);
    }
}