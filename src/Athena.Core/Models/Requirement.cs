using System;

namespace Athena.Core.Models
{
    public class Requirement : IUniqueObject<Guid>, IEquatable<Requirement>
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

        public bool Equals(Requirement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) &&
                   string.Equals(Name, other.Name) &&
                   string.Equals(Description, other.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Requirement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}