using System;

namespace Athena.Core.Models.Identity
{
    public class AthenaRole : IEquatable<AthenaRole>
    {
        public static readonly Guid AdminRole = new Guid("f839e0f0-e29d-4c89-b406-51a52dbb08b5");
        public const string AdminRoleName = "Administrator";
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public bool Equals(AthenaRole other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) && string.Equals(Name, other.Name) && string.Equals(NormalizedName, other.NormalizedName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((AthenaRole) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NormalizedName != null ? NormalizedName.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}