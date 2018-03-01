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
    [Route("api/v1/[Controller]")]
    public class OfferingController : Controller
    {
        private readonly IOfferingReository offerings;
        private readonly IMeetingRepository meetings;
        private readonly ICourseRepository courses;

        public OfferingController(IOfferingReository offeringsRepository, IMeetingRepository meetingsRepository, ICourseRepository coursesRepository)
        {
            offerings = offeringsRepository ?? throw new ArgumentNullException(nameof(offeringsRepository));
            meetings = meetingsRepository ?? throw new ArgumentNullException(nameof(meetingsRepository));
            courses = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
        }
        [HttpPost]
        public async Task AddOffering([FromBody] Offering offering) => await offerings.AddAsync(offering);
        
        [HttpGet("{id}")]
        public async Task<Offering> GetOffering(Guid id) => await offerings.GetAsync(id);

        [HttpPut("{id}")]
        public async Task EditOffering(Guid id, [FromBody] Offering offering)
        {
            if (id != offering.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {offering.Id}");
            }
            await offerings.EditAsync(offering);
        }

        [HttpDelete("{id}")]
        public async Task DeleteOffering(Guid id)
        {
            var offering = await offerings.GetAsync(id);
            if (offering == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to delete {offering} that does not exist");
            }
            await offerings.DeleteAsync(offering);
        }

        [HttpGet("api/v1/course/{id}/offering/{offeringId}")]
        public async Task<IEnumerable<Offering>> GetOfferingsForCourseAsync(Course course)
        {
            if (course == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get offering for {course} that doesn't exist");
            }
            return (await offerings.GetOfferingsForCourseAsync(course));
        }

        [HttpPost("api/v1/course/{id}/offering/{offeringId}/meeting")]
        public async Task AddMeetingAsync(Offering offering, Meeting meeting)
        {                                       
            if (meeting == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add meeting {meeting} where meeting does not exist");
            }
            await offerings.AddMeetingAsync(offering, meeting);
        }

        [HttpDelete("api/v1/course/{id}/offering/{offeringId}/meeting")]
        public async Task RemoveMeetingAsync(Offering offering, Meeting meeting)
        {
            if (meeting == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to remove meeting {meeting} where meeting does not exist");
            }
            await offerings.RemoveMeetingAsync(offering, meeting);
        }

        [HttpGet("api/v1/course/{id}/offering/{offeringId}/meeting")]
        public async Task<IEnumerable<Meeting>> GetMeetingsForOfferingAsync(Offering offering)
        {
            if (offering == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to get meetings for {offering} where offering doesn't exist");
            }
            return (await meetings.GetMeetingsForOfferingAsync(offering));
        }

        [HttpPost("api/v1/course/{id}/offering/{offeringId}")]
        public async Task AddOfferingAsync(Course course, Offering offering)
        {
            if (offering == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to add offering {offering} to course {course} that doesn't exist");
            }
            await courses.AddOfferingAsync(course, offering);
        }

        [HttpDelete("api/v1/course/{id}/offering/{offeringId}")]
        public async Task RemoveOfferingAsync(Course course, Offering offering)
        {
            if (offering == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to remove offering {offering} to course {course} that doesn't exist");
            }
            await courses.RemoveOfferingAsync(course, offering);
        }
    }
}