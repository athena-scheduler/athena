using System;
using System.Collections.Generic;
using System.Linq;

namespace Athena.Core.Repositories
{
    public class ProgramSearchOptions : IEquatable<ProgramSearchOptions>
    {
        public string Query { get; set; }
        public Guid Student { get; set; }

        public bool Equals(ProgramSearchOptions other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Query, other.Query) &&
                   Student.Equals(other.Student);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProgramSearchOptions) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Query != null ? Query.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Student.GetHashCode();
                return hashCode;
            }
        }
    }
}