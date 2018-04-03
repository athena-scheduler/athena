using System;
using Athena.Core.Models;
using Newtonsoft.Json;

namespace Athena.Models
{
    /// <summary>
    /// POCO for https://fullcalendar.io/docs/event-object
    /// </summary>
    public class ScheduleEntry
    {
        public string Id => $"{OfferingId}/{MeetingId}";
        public readonly Guid OfferingId;
        public readonly Guid MeetingId;

        public readonly string Title;
        
        [JsonProperty("dow")]
        public readonly DayOfWeek[] Day;
        
        public readonly TimeSpan Start;
        public readonly TimeSpan End;
        
        public ScheduleEntry(Offering offering, Meeting meeting)
        {
            OfferingId = offering.Id;
            MeetingId = meeting.Id;

            Title = $"{offering.Course.Name} - At {offering.Campus.Name} in {meeting.Room}";

            Day = new []{ meeting.Day };

            Start = meeting.Time;
            End = meeting.Time.Add(meeting.Duration);
        }
    }
}