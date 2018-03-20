using System;
using System.Collections.Generic;
using System.Linq;

namespace Athena.Core.Repositories
{
    public class ProgramSearchOptions
    {
        public string Query { get; set; }
        public IEnumerable<Guid> InstitutionIds { get; set; } = Enumerable.Empty<Guid>();
    }
}