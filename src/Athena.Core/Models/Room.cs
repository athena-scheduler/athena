using System;
using System.Collections.Generic;

namespace Athena.Core.Models
{
    public class Room : IUniqueObject<Guid>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the room
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// A description of the room
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The campus the room is on
        /// </summary>
        public Campus Campus { get; set; }
        
        /// <summary>
        /// A list of capabilities the room provides
        /// </summary>
        public List<string> Capabilities { get; } = new List<string>();
        
    }
}