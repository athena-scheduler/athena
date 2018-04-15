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
        private static readonly DayOfWeek[] EXTERNAL_DOW =
        {
            DayOfWeek.Sunday,
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday
        };
        
        public string Id => $"{OfferingId}/{MeetingId}";
        public readonly Guid OfferingId;
        public readonly Guid MeetingId;

        public readonly string Title;
        public readonly string Tooltip;
        
        [JsonProperty("dow")]
        public readonly DayOfWeek[] Day;

        public readonly bool AllDay;
        
        public readonly TimeSpan Start;
        public readonly TimeSpan End;
        
        public ScheduleEntry(Offering offering, Meeting meeting)
        {
            OfferingId = offering.Id;
            MeetingId = meeting.Id;

            Title = offering.Course.Name;

            Tooltip = meeting.External ? offering.Course.Name : $"{offering.Course.Name} - At {offering.Campus.Name} in {meeting.Room}";

            Day = meeting.External ? EXTERNAL_DOW : new []{ meeting.Day };

            Start = meeting.Time;
            End = meeting.Time.Add(meeting.Duration);

            AllDay = meeting.External;
        }
    }
}