using System;

namespace Athena.Core.Models
{
    public class Requirement : IUniqueObject<Guid>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the requirement
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// A description of the requirement
        /// </summary>
        public string Description { get; set; }
    }
}