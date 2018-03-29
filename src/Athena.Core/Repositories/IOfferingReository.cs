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

        /// <summary>
        /// Get all courses being taken by a student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        Task<IEnumerable<Offering>> GetInProgressOfferingsForStudentAsync(Student student);

        /// <summary>
        /// Enroll the student in the specified offering
        /// </summary>
        /// <param name="student">The student to enroll</param>
        /// <param name="offering">The offering to enroll in</param>
        Task EnrollStudentInOfferingAsync(Student student, Offering offering);
        
        /// <summary>
        /// Unenroll the student in the specified offering
        /// </summary>
        /// <param name="student">The student to enroll</param>
        /// <param name="offering">The offering to unenroll from</param>
        Task UnenrollStudentInOfferingAsync(Student student, Offering offering);
    }
}