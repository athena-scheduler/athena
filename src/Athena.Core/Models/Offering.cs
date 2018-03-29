using System;
using System.Collections.Generic;

namespace Athena.Core.Models
{
    public class Offering : IUniqueObject<Guid>, IEquatable<Offering>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <summary>
        /// The course the offering is for
        /// </summary>
        public Course Course { get; set; }
        
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
        /// The meetings for this offering
        /// </summary>
        public IEnumerable<Meeting> Meetings { get; set; }

        public bool Equals(Offering other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) &&
                   Equals(Course, other.Course) &&
                   Equals(Campus, other.Campus) &&
                   Start.Date.Equals(other.Start.Date) &&
                   End.Date.Equals(other.End.Date);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Offering) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Course != null ? Course.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Campus != null ? Campus.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Start.GetHashCode();
                hashCode = (hashCode * 397) ^ End.GetHashCode();
                return hashCode;
            }
        }
    }
}