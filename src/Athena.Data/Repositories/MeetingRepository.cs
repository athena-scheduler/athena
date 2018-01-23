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
    public class MeetingRepository : PostgresRepository, IMeetingRepository
    {
        public MeetingRepository(IDbConnection db) : base(db)
        {
        }

        public async Task<Meeting> GetAsync(Guid id) =>
            (await _db.QueryAsync<Meeting>("SELECT * FROM meetings WHERE id = @id", new {id}))
                .FirstOrDefault();

        public async Task AddAsync(Meeting obj) =>
            await _db.InsertUniqueAsync(
                "INSERT INTO meetings VALUES (@id, @day, @time, @duration, @room)",
                new { obj.Id, obj.Day, obj.Time, obj.Duration, obj.Room }
            );

        public async Task EditAsync(Meeting obj) =>
            await _db.ExecuteAsync(@"
                UPDATE meetings SET
                    day = @day,
                    time = @time,
                    duration = @duration,
                    room = @room
                WHERE id = @id",
                new {obj.Day, obj.Time, obj.Duration, obj.Room, obj.Id}
            );

        public async Task DeleteAsync(Meeting obj) =>
            await _db.ExecuteAsync("DELETE FROM meetings WHERE id = @id", new {obj.Id});

        public async Task<IEnumerable<Meeting>> GetMeetingsForOfferingAsync(Offering offering) =>
            await _db.QueryAsync<Meeting>(@"
                SELECT m.id,
                       m.day
                       m.time,
                       m.duration,
                       m.room
                FROM meetings m
                    LEFT JOIN offering_x_meeting link
                        ON m.id = link.meeting
                WHERE link.offering = @id",
                new {offering.Id}
            );
    }
}