using System;
using System.Collections.Generic;

namespace Athena.Core.Models
{
    public class Program : IUniqueObject<Guid>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }
        
        /// <summary>
        /// The name of the Program
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// A description of the Program
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Courses that must be completed to count this program as completed
        /// </summary>
        public List<Requirement> Requirements { get; } = new List<Requirement>(); 
    }
}