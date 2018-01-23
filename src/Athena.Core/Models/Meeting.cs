using System;

namespace Athena.Core.Models
{
    public class Meeting : IUniqueObject<Guid>, IEquatable<Meeting>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <summary>
        /// The day of the week the meeting is on
        /// </summary>
        public DayOfWeek Day { get; set; }
        /// <summary>
        /// The time of day the meeting is at
        /// </summary>
        public DateTime Time { get; set; }
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
                   Day == other.Day &&
                   Time.Equals(other.Time) && 
                   Duration.Equals(other.Duration) &&
                   string.Equals(Room, other.Room);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Meeting) obj);
        }
    }
}