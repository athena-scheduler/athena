using System.Net;
using Athena.Core.Models;

namespace Athena.Exceptions
{
    public class OfferingConflictException : ApiException
    {
        public OfferingConflictException(Offering conflict, Meeting conflictingTimeSlot)
            : base(HttpStatusCode.Conflict, "Requested offering conflicts with an in-progress offering", new
            {
                conflict,
                conflictingTimeSlot
            })
        {
        }
    }
}