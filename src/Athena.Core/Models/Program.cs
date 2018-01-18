using System;

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
    }
}