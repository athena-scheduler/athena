using System;
using System.Security.Claims;
using System.Security.Principal;
using Athena.Core.Models.Identity;

namespace Athena.Models.Identity
{
    public class AthenaPrincipal : ClaimsPrincipal
    {
        public readonly AthenaUser AthenaUser;

        public AthenaPrincipal(IPrincipal _base, AthenaUser user) : base(_base) =>
            AthenaUser = user ?? throw new ArgumentNullException(nameof(user));
    }
}