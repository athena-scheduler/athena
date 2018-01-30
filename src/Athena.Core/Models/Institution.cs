using System;

namespace Athena.Core.Models
{
    public class Institution : IUniqueObject<Guid>, IEquatable<Institution>
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

        public bool Equals(Institution other)
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
            return obj.GetType() == this.GetType() && Equals((Institution) obj);
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