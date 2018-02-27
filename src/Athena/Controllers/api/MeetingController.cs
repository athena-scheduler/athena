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
    public class MeetingController : Controller
    {
        private readonly IMeetingRepository meetings;
        private readonly IOfferingReository offerings;

        public MeetingController (IMeetingRepository meetingsRepository, IOfferingReository offeringsRepository)
        {
            meetings = meetingsRepository ?? throw new ArgumentNullException(nameof(meetingsRepository));
            offerings = offeringsRepository ?? throw new ArgumentNullException(nameof(offeringsRepository));
        }

        [HttpPost]
        public async Task AddMeeting([FromBody] Meeting meeting) => await meetings.AddAsync(meeting);

        [HttpGet("{id}")]
        public async Task<Meeting> GetMeeting(Guid id) => await meetings.GetAsync(id);

        [HttpPut("{id}")]
        public async Task EditMeeting(Guid id, [FromBody] Meeting meeting)
        {
            if (id != meeting.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {meeting.Id}");
            }
            await meetings.EditAsync(meeting);
        }

        [HttpDelete("{id}")]
        public async Task DeleteMeeting(Guid id)
        {
            var meeting = await meetings.GetAsync(id);
            if (meeting == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to delete {meeting} that does not exist");
            }
            await meetings.DeleteAsync(meeting);
        }
    }
}