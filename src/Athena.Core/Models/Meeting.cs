using System;

namespace Athena.Core.Models
{
    public class Meeting : IUniqueObject<Guid>
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
    }
}