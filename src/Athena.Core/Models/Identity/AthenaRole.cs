using System;

namespace Athena.Core.Models.Identity
{
    public class AthenaRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}