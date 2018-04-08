using System;

namespace Athena.Core.Repositories
{
    public class CourseSearchOptions : IEquatable<CourseSearchOptions>
    {
        public string Query { get; set; }
        public bool Completed { get; set; }
        public bool IncludeInProgress { get; set; } = true;
        public Guid StudentId { get; set; }

        public bool Equals(CourseSearchOptions other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Query, other.Query) &&
                   StudentId.Equals(other.StudentId) &&
                   Completed == other.Completed &&
                   IncludeInProgress == other.IncludeInProgress;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CourseSearchOptions)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StudentId.GetHashCode();
                hashCode = (hashCode * 397) ^ (Query != null ? Query.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Completed.GetHashCode());
                hashCode = (hashCode * 397) ^ (IncludeInProgress.GetHashCode());
                return hashCode;
            }
        }
    }
}
