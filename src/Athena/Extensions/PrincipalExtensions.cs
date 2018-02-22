using System.Security.Claims;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using Athena.Models.Identity;

namespace Athena.Extensions
{
    public static class PrincipalExtensions
    {
        public static AthenaUser ToAthenaUser(this ClaimsPrincipal user) =>
            (user as AthenaPrincipal)?.AthenaUser;
        
        public static Student GetStudent(this ClaimsPrincipal user) =>
            user.ToAthenaUser()?.Student;
    }
}