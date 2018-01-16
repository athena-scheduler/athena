using System;
using System.Collections.Generic;

namespace Athena.Core.Models
{
    public class Offering : IUniqueObject<Guid>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <summary>
        /// The campus that this offering is on
        /// </summary>
        public Campus Campus { get; set; }
        
        /// <summary>
        /// The date that the offering starts on
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// The date that the offering ends on
        /// </summary>
        public DateTime End { get; set; }
        
        /// <summary>
        /// A list of meeting dates and times for the offering
        /// </summary>
        public List<Meeting> Meetings { get; set; } = new List<Meeting>();
    }
}