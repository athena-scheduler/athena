using System;
using System.Collections.Generic;

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

        /// <summary>
        /// The institutions that offer courses on this campus
        /// </summary>
        public List<Institution> Institutions { get; } = new List<Institution>();

        /// <summary>
        /// The rooms on this campus
        /// </summary>
        public List<Room> Rooms { get; } = new List<Room>();
    }
}