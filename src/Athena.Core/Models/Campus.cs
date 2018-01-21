using System;

namespace Athena.Core.Models
{
    public class Campus : IUniqueObject<Guid>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the campus
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// A description of the campus
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The address of the campus
        /// </summary>
        public string Location { get; set; }
    }
}