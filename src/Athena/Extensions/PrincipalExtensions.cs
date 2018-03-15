using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using Athena.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Athena.Extensions
{
    public static class PrincipalExtensions
    {
        public static AthenaUser ToAthenaUser(this ClaimsPrincipal user) =>
            (user as AthenaPrincipal)?.AthenaUser;
        
        public static Student GetStudent(this ClaimsPrincipal user) =>
            user.ToAthenaUser()?.Student;

        public static async Task<ClaimsPrincipal> TryGetAthenaPrincipal(this ClaimsPrincipal user, UserManager<AthenaUser> userManager)
        {
            if (user is AthenaPrincipal a)
            {
                return a;
            }
            
            var athenaUser = await (userManager ?? throw new ArgumentNullException(nameof(userManager)))
                .GetUserAsync(user);

            return athenaUser == null ? user : new AthenaPrincipal(user, athenaUser);
        }
    }
}