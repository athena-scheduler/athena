using System;

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
    }
}