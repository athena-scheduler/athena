using System;

namespace Athena.Core.Models
{
    /// <summary>
    /// Represents a student who is enrolled in one or more <see cref="Program"/> at one or more
    /// <see cref="Institution"/>s.
    /// </summary>
    public class Student : IUniqueObject<Guid>, IEquatable<Student>
    {
        /// <inheritdoc />
        public Guid Id { get; set; } = Guid.Empty;

        /// <summary>
        /// The name of the student
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The primary email address of the student
        /// </summary>
        public string Email { get; set; }

        public bool Equals(Student other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) &&
                   string.Equals(Name, other.Name) &&
                   string.Equals(Email, other.Email);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Student) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Email != null ? Email.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}