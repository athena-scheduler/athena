using System;
using System.Collections.Generic;

namespace Athena.Core.Models
{
    public class Institution : IUniqueObject<Guid>
    {
        /// <inheritdoc />
        public Guid Id { get; set; } = Guid.Empty;
        
        /// <summary>
        /// The name of the institution
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A description or motto for the institution
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The campuses on which the institution offers coursese
        /// </summary>
        public List<Campus> Campuses { get; } = new List<Campus>();

        /// <summary>
        /// The programs offerred by the institution
        /// </summary>
        public List<Program> Programs { get; } = new List<Program>();
        
        /// <summary>
        /// The courses offerred by the institution
        /// </summary>
        public List<Course> Courses { get; } = new List<Course>();
    }
}