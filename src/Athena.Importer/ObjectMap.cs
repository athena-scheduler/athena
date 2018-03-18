using System;
using System.Collections.Generic;

namespace Athena.Importer
{
    public class ObjectMap
    {
        public Dictionary<Guid, IEnumerable<Guid>> CampusInstitutions { get; set; }
        public Dictionary<Guid, IEnumerable<Guid>> CourseOfferings { get; set; }
        public Dictionary<Guid, IEnumerable<Guid>> CourseRequirements { get; set; }
        public Dictionary<Guid, IEnumerable<Guid>> CoursePrereqs { get; set; }
        public Dictionary<Guid, IEnumerable<Guid>> CourseConcurrentPrereqs { get; set; }
        public Dictionary<Guid, IEnumerable<Guid>> OfferingMeetings { get; set; }
        public Dictionary<Guid, IEnumerable<Guid>> ProgramRequirements { get; set; }
    }
}