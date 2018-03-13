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
using Athena.Extensions;

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
        public async Task AddMeetingAsync(Guid offeringId, Guid meetingId)
        {
            var offering = (await _offerings.GetAsync(offeringId)).NotFoundIfNull();
            var meeting = (await _meetings.GetAsync(meetingId)).NotFoundIfNull();

            await _offerings.AddMeetingAsync(offering, meeting);
        }

        [HttpDelete("{meetingId}")]
        public async Task RemoveMeetingAsync(Guid offeringId, Guid meetingId)
        {
            var offering = (await _offerings.GetAsync(offeringId)).NotFoundIfNull();
            var meeting = (await _meetings.GetAsync(meetingId)).NotFoundIfNull();

            await _offerings.RemoveMeetingAsync(offering, meeting);
        }

        [HttpGet]
        public async Task<IEnumerable<Meeting>> GetMeetingsForOfferingAsync(Guid offeringId)
        {
            var offering = (await _offerings.GetAsync(offeringId)).NotFoundIfNull();

            return await _meetings.GetMeetingsForOfferingAsync(offering);
        }
    }
}
