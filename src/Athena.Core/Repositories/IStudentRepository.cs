using System;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IStudentRepository : IRepository<Student, Guid>
    {
        /// <summary>
        /// Register the student for the specified program
        /// </summary>
        /// <param name="student">The student to register</param>
        /// <param name="program">The program to register the student for</param>
        Task RegisterForProgramAsync(Student student, Program program);

        /// <summary>
        /// Remove the student from the specified program
        /// </summary>
        /// <param name="student">The student to remove</param>
        /// <param name="program">The program to unregister from</param>
        Task UnregisterForProgramAsync(Student student, Program program);
    }
}