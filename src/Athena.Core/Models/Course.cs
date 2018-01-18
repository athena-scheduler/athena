using System;

namespace Athena.Core.Models
{
    public class Course : IUniqueObject<Guid>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }
        
        /// <summary>
        /// The name of the course
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The Institution that offers this course
        /// </summary>
        public Institution Institution { get; set; }
    }
}