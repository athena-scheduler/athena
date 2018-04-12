using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Repositories;
using Athena.Exceptions;

namespace Athena.Extensions
{
    public static class EnumerableExtensions
    {
        public static async Task CheckForConflictingTimeSlots(this Offering offering, Student student, IOfferingReository _offerings)
        {
            foreach (var inProgressOffering in await _offerings.GetInProgressOfferingsForStudentAsync(student))
            {
                foreach (var inProgressMeeting in inProgressOffering.Meetings)
                {
                    // Clever solutin from https://stackoverflow.com/a/13513973/8723823
                    if (offering.Meetings.Any(m => m.Day == inProgressMeeting.Day && inProgressMeeting.Time < m.End && m.Time < inProgressMeeting.End))
                    {
                        throw new OfferingConflictException(inProgressOffering, inProgressMeeting);
                    }
                }
            }
        }
    }
}