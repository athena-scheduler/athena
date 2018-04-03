using System;
using System.Collections.Generic;
using System.Text;

namespace Athena.Core.Repositories
{
    public class CourseSearchOptions
    {
        public string Query { get; set; }
        public bool Completed { get; set; }
        public Guid StudentId { get; set; }

    }
}
