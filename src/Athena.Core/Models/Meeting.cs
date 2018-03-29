using System;

namespace Athena.Core.Models
{
    public class Meeting : IUniqueObject<Guid>, IEquatable<Meeting>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }
        
        /// <summary>
        /// The ID of the offering this meeting is associated with
        /// </summary>
        public Guid Offering { get; set; }

        /// <summary>
        /// The day of the week the meeting is on
        /// </summary>
        public DayOfWeek Day { get; set; }
        /// <summary>
        /// The time of day the meeting is at
        /// </summary>
        public TimeSpan Time { get; set; }
        /// <summary>
        /// The durationof the meeting
        /// </summary>
        public TimeSpan Duration { get; set; }
        
        /// <summary>
        /// The room the meeting is in
        /// </summary>
        public string Room { get; set; }

        public bool Equals(Meeting other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) &&
                   Offering.Equals(other.Offering) &&
                   Day == other.Day &&
                   Math.Floor(Time.TotalMilliseconds).Equals(Math.Floor(other.Time.TotalMilliseconds)) &&
                   Math.Floor(Duration.TotalMilliseconds).Equals(Math.Floor(other.Duration.TotalMilliseconds)) &&
                   string.Equals(Room, other.Room);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Meeting) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ Offering.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Day;
                hashCode = (hashCode * 397) ^ Time.GetHashCode();
                hashCode = (hashCode * 397) ^ Duration.GetHashCode();
                hashCode = (hashCode * 397) ^ (Room != null ? Room.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}