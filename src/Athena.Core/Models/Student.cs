using System;
using System.Collections.Generic;

namespace Athena.Core.Models
{
    /// <summary>
    /// Represents a student who is enrolled in one or more <see cref="Program"/> at one or more
    /// <see cref="Institution"/>s.
    /// </summary>
    public class Student : IUniqueObject<Guid>
    {
        /// <inheritdoc />
        public Guid Id { get; set; } = Guid.Empty;

        /// <summary>
        /// The name of the student
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The primary email address of the student
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The institutions the student belongs to
        /// </summary>
        public List<Institution> Institutions { get; } = new List<Institution>();
        /// <summary>
        /// The programs the student is enrolled in
        /// </summary>
        public List<Program> Programs { get; } = new List<Program>();
        
        /// <summary>
        /// The courses the student has already completed
        /// </summary>
        public List<Course> CompletedCourses { get; } = new List<Course>();
        /// <summary>
        /// The courses the student is presently enrolled in
        /// </summary>
        public List<Course> InProgressCourses { get; } = new List<Course>();
    }
}