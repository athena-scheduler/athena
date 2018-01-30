using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IRequirementRepository : IRepository<Requirement, Guid>
    {
        /// <summary>
        /// Get the requirements the specified course satisfies
        /// </summary>
        /// <param name="course">The course to get requirements for</param>
        /// <returns>An IEnumerable of requirements</returns>
        /// <remarks>
        /// Modify this collection with <see cref="ICourseRepository.AddSatisfiedRequirementAsync"/>
        /// and <see cref="ICourseRepository.RemoveSatisfiedRequirementAsync"/>
        /// </remarks>
        Task<IEnumerable<Requirement>> GetRequirementsCourseSatisfiesAsync(Course course);

        /// <summary>
        /// Get the required courses that must be taken before the provided course can be taken
        /// </summary>
        /// <param name="course">The course to get prereqs for</param>
        /// <returns>An IEnumerable of requirements</returns>
        /// <remarks>
        /// Modify this collection with <see cref="ICourseRepository.AddPrerequisiteAsync"/>
        /// and <see cref="ICourseRepository.RemovePrerequisiteAsync"/>
        /// </remarks>
        Task<IEnumerable<Requirement>> GetPrereqsForCourseAsync(Course course);

        /// <summary>
        /// Get the required courses that must be taken before or concurrently with the provided course
        /// </summary>
        /// <param name="course">The course to get prereqs for</param>
        /// <returns>An IEnumerable of requirements</returns>
        /// <remarks>
        /// Modify this collection with <see cref="ICourseRepository.AddConcurrentPrerequisiteAsync"/>
        /// and <see cref="ICourseRepository.RemoveConcurrentPrerequisiteAsync"/>
        /// </remarks>
        Task<IEnumerable<Requirement>> GetConcurrentPrereqsAsync(Course course);

        /// <summary>
        /// Get the required courses to consider the provided Program as completed
        /// </summary>
        /// <param name="program">The program to get requirements for</param>
        /// <returns>An IEnumerable of requirements</returns>
        /// <remarks>
        /// Modify this collection with <see cref="IProgramRepository.AddRequirementAsync"/>
        /// and <see cref="IProgramRepository.RemoveRequirementAsync"/>
        /// </remarks>
        Task<IEnumerable<Requirement>> GetRequirementsForProgramAsync(Program program);
    }
}