﻿using System;
using Microsoft.AspNetCore.Identity;

namespace Athena.Core.Models.Identity
{
    public class AthenaUser : IdentityUser<Guid>, IEquatable<AthenaUser>
    {
        public Student Student { get; set; }
        public string ApiKey { get; set; }
        
        public bool Equals(AthenaUser other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) &&
                   Student.Equals(other.Student) &&
                   string.Equals(UserName, other.UserName) &&
                   string.Equals(NormalizedUserName, other.NormalizedUserName) &&
                   string.Equals(Email, other.Email) &&
                   string.Equals(NormalizedEmail, other.NormalizedEmail) &&
                   string.Equals(ApiKey, other.ApiKey) &&
                   string.Equals(SecurityStamp, other.SecurityStamp) &&
                   EmailConfirmed == other.EmailConfirmed;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((AthenaUser) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) & (Student?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (UserName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (NormalizedUserName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Email?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (NormalizedEmail?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (ApiKey?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (SecurityStamp?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (EmailConfirmed.GetHashCode());
                return hashCode;
            }
        }
    }
}