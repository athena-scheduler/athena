using System;

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
    }
}