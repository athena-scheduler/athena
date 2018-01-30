using System;

namespace Athena.Core.Models
{
    public class Course : IUniqueObject<Guid>, IEquatable<Course>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }
        
        /// <summary>
        /// The name of the course
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The Institution that offers this course
        /// </summary>
        public Institution Institution { get; set; }

        public bool Equals(Course other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) &&
                   string.Equals(Name, other.Name) &&
                   Equals(Institution, other.Institution);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Course) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Institution != null ? Institution.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}