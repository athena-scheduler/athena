using System;

namespace Athena.Core.Models
{
    public class Program : IUniqueObject<Guid>, IEquatable<Program>
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
        
        /// <summary>
        /// The institution that offers this institution
        /// </summary>
        public Institution Institution { get; set; }

        public bool Equals(Program other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) &&
                   string.Equals(Name, other.Name) &&
                   string.Equals(Description, other.Description) &&
                   Equals(Institution, other.Institution);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Program) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Institution != null ? Institution.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}