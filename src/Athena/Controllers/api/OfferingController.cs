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
    [Route("api/v1/[Controller]")]
    public class OfferingController : Controller
    {
        private readonly IOfferingReository _offerings;
        private readonly IMeetingRepository _meetings;
        private readonly ICourseRepository _courses;

        public OfferingController(IOfferingReository offeringsRepository, IMeetingRepository meetingsRepository, ICourseRepository coursesRepository)
        {
            _offerings = offeringsRepository ?? throw new ArgumentNullException(nameof(offeringsRepository));
            _meetings = meetingsRepository ?? throw new ArgumentNullException(nameof(meetingsRepository));
            _courses = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
        }

        [HttpPost]
        public async Task AddOffering([FromBody] Offering offering) => await _offerings.AddAsync(offering);
        
        [HttpGet("{id}")]
        public async Task<Offering> GetOffering(Guid id) => await _offerings.GetAsync(id) ?? throw new ApiException(HttpStatusCode.NotFound, "offering not found");

        [HttpPut("{id}")]
        public async Task EditOffering(Guid id, [FromBody] Offering offering)
        {
            if (id != offering.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Tried to edit {id} but got a model for {offering.Id}");
            }
            await _offerings.EditAsync(offering);
        }

        [HttpDelete("{id}")]
        public async Task DeleteOffering(Guid id)
        {
            var offering = (await _offerings.GetAsync(id)).NotFoundIfNull();
            
            await _offerings.DeleteAsync(offering);
        }

        
    }
}