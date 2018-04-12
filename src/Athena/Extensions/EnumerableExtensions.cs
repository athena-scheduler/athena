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
        public static async Task CheckForConflictingTimeSlots(this Offering offering, Student student, IOfferingReository offerings)
        {
            foreach (var inProgressOffering in await offerings.GetInProgressOfferingsForStudentAsync(student))
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

        public static async Task CheckForMetPrerequisites(this Offering offering, Student student, IRequirementRepository requirements)
        {
            var required = (await requirements.GetPrereqsForCourseAsync(offering.Course)).ToList();
            var concurrent = (await requirements.GetConcurrentPrereqsAsync(offering.Course)).ToList();

            var taken = (await requirements.GetCompletedRequirementsForStudentAsync(student)).ToList();
            var inProgress = (await requirements.GetInProgressRequirementsForStudentAsync(student)).ToList();

            var unmet = required.Where(r => !taken.Contains(r)).ToList();
            var unmetConcurrent = concurrent.Where(r => !taken.Contains(r) && !inProgress.Contains(r)).ToList();

            if (unmet.Count + unmetConcurrent.Count != 0)
            {
                throw new UnmetDependencyException(offering.Course, unmet, unmetConcurrent);
            }
        }
    }
}