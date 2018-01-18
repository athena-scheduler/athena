using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IOfferingReository : IRepository<Offering, Guid>
    {
        /// <summary>
        /// Get all offerings for the specified course
        /// </summary>
        /// <param name="course">The course to get offerings for</param>
        /// <returns>An IEnumerable of offerings</returns>
        Task<IEnumerable<Offering>> GetOfferingsForCourseAsync(Course course);
    }
}