using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Repositories;
using Athena.Data.Extensions;
using Dapper;

namespace Athena.Data.Repositories
{
    public class OfferingRepository : PostgresRepository, IOfferingReository
    {
        public OfferingRepository(IDbConnection db) : base(db)
        {
        }

        public async Task<Offering> GetAsync(Guid id) =>
            (await _db.QueryAsync<Offering, Campus, Offering>(@"
                 SELECT o.id,
                        o.start,
                        o.end,
                        c.id,
                        c.name,
                        c.description,
                        c.location
                 FROM offerings o
                    LEFT JOIN campuses c
                        ON o.campus = c.id
                 WHERE o.id = @id",
                _mapOffering,
                new { id }
            )).FirstOrDefault();

        public async Task AddAsync(Offering obj) =>
            await _db.InsertUniqueAsync(
                "INSERT INTO offerings VALUES (@id, @campus, @start, @end)",
                new {obj.Id, campus = obj.Campus.Id, obj.Start, obj.End}
            );
        
        public async Task EditAsync(Offering obj) =>
            await _db.ExecuteAsync(
                "UPDATE offerings SET campus = @campus, start = @start, \"end\" = @end WHERE id = @id",
                new { campus = obj.Campus.Id, obj.Start, obj.End, obj.Id }
            );

        public async Task DeleteAsync(Offering obj) =>
            await _db.ExecuteAsync(
                "DELETE FROM offerings WHERE id = @id",
                new {id = obj.Id}
            );

        public async Task<IEnumerable<Offering>> GetOfferingsForCourseAsync(Course course) =>
            await _db.QueryAsync<Offering, Campus, Offering>(@"
                SELECT o.id,
                       o.start,
                       o.end,
                       c.id,
                       c.name,
                       c.description,
                       c.location
                FROM offerings o
                   LEFT JOIN campuses c
                       ON o.campus = c.id
                   LEFT JOIN course_x_offering link
                       ON o.id = link.offering
                WHERE link.course = @id",
                _mapOffering,
                new { course.Id }
            );

        public async Task AddMeetingAsync(Offering offering, Meeting meeting) =>
            await _db.InsertUniqueAsync(
                "INSERT INTO offering_x_meeting VALUES (@offering, @meeting)",
                new { offering = offering.Id, meeting = meeting.Id }
            );

        public async Task RemoveMeetingAsync(Offering offering, Meeting meeting) =>
            await _db.ExecuteAsync(
                "DELETE FROM offering_x_meeting WHERE offering = @offering AND meeting = @meeting",
                new { offering = offering.Id, meeting = meeting.Id }
            );

        private static Offering _mapOffering(Offering o, Campus c)
        {
            o.Campus = c;
            return o;
        }
    }
}