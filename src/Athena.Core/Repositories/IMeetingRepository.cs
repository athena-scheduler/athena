using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IMeetingRepository : IRepository<Meeting, Guid>
    {
        /// <summary>
        /// Get all meetings for the specified offering
        /// </summary>
        /// <param name="offering">The offering to get meetings for</param>
        /// <returns>An IEnumerable of Meetings</returns>
        Task<IEnumerable<Meeting>> GetMeetingsForOfferingAsync(Offering offering);
    }
}