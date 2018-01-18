using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface ICourseRepositorycs : IRepository<Course, Guid>
    {
        /// <summary>
        /// Get all courses offered by the provided institution
        /// </summary>
        /// <param name="institution">The institution to get courses for</param>
        /// <returns>An IEnumerable of Courses</returns>
        Task<IEnumerable<Course>> GetCoursesForInstitutionAsync(Institution institution);

        /// <summary>
        /// Get all completed courses that the provided student has taken 
        /// </summary>
        /// <param name="student">The student to get completed courses for</param>
        /// <returns>An IEnumerable of Courses</returns>
        Task<IEnumerable<Course>> GetCompletedCoursesForStudentAsync(Student student);

        /// <summary>
        /// Get all in-progress courses for the specified students
        /// </summary>
        /// <param name="student">The student to get courses for</param>
        /// <returns>An IEnumerable of Courses</returns>
        Task<IEnumerable<Course>> GetInProgressCoursesForStudentAsync(Student student);
    }
}