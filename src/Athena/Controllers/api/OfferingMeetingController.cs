﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;
using Athena.Core.Models;
using Athena.Extensions;

namespace Athena.Controllers.api
{
    [Route("api/v1/offering/{offeringId}/meeting")]
    public class OfferingMeetingController : AthenaApiController
    {
        private readonly IOfferingReository _offerings;
        private readonly IMeetingRepository _meetings;

        public OfferingMeetingController(IOfferingReository offeringsRepository, IMeetingRepository meetingsRepository)
        {
            _offerings = offeringsRepository ?? throw new ArgumentNullException(nameof(offeringsRepository));
            _meetings = meetingsRepository ?? throw new ArgumentNullException(nameof(meetingsRepository));
        }

        [HttpGet]
        public async Task<IEnumerable<Meeting>> GetMeetingsForOfferingAsync(Guid offeringId)
        {
            var offering = (await _offerings.GetAsync(offeringId)).NotFoundIfNull();

            return await _meetings.GetMeetingsForOfferingAsync(offering);
        }
    }
}
