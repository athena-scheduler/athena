using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IProgramRepository : ISearchableRepository<Program, Guid, ProgramSearchOptions>
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
        /// <remarks>
        /// Modify this collection with <see cref="IStudentRepository.RegisterForProgramAsync"/>
        /// and <see cref="IStudentRepository.UnregisterForProgramAsync"/>
        /// </remarks>
        Task<IEnumerable<Program>> GetProgramsForStudentAsync(Student student);

        /// <summary>
        /// Add the specified requirement to the program
        /// </summary>
        /// <param name="program">The program to alter</param>
        /// <param name="requirement">The requirement to add</param>
        Task AddRequirementAsync(Program program, Requirement requirement);
        
        /// <summary>
        /// Remove the specified requirement from the program
        /// </summary>
        /// <param name="program">The program to alter</param>
        /// <param name="requirement">The requirement to remove</param>
        Task RemoveRequirementAsync(Program program, Requirement requirement);
    }
}