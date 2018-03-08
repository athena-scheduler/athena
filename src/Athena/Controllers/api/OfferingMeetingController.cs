using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Exceptions;
using System.Net;
namespace Athena.Controllers.api
{
    [Route("api/v1/offering/{offeringId}/meeting")]
    public class OfferingMeetingController : Controller
    {
        private readonly IOfferingReository _offerings;
        private readonly IMeetingRepository _meetings;

        public OfferingMeetingController(IOfferingReository offeringsRepository, IMeetingRepository meetingsRepository)
        {
            _offerings = offeringsRepository ?? throw new ArgumentNullException(nameof(offeringsRepository));
            _meetings = meetingsRepository ?? throw new ArgumentNullException(nameof(meetingsRepository));
        }

        [HttpPost("{meetingId}")]
        public async Task AddMeetingAsync(Offering offering, Meeting meeting)
        {
            if (meeting == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add meeting {meeting} where meeting does not exist");
            }
            await _offerings.AddMeetingAsync(offering, meeting);
        }

        [HttpDelete("{meetingId}")]
        public async Task RemoveMeetingAsync(Offering offering, Meeting meeting)
        {
            if (meeting == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to remove meeting {meeting} where meeting does not exist");
            }
            await _offerings.RemoveMeetingAsync(offering, meeting);
        }

        [HttpGet]
        public async Task<IEnumerable<Meeting>> GetMeetingsForOfferingAsync(Offering offering)
        {
            if (offering == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Tried to get meetings for {offering} where offering doesn't exist");
            }
            return await _meetings.GetMeetingsForOfferingAsync(offering);
        }
    }
}
