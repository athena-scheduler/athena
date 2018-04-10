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
            await _db.ExecuteCheckedAsync(
                "INSERT INTO meetings VALUES (@id, @day, @time, @duration, @room, @offering)",
                new { obj.Id, obj.Day, obj.Time, obj.Duration, obj.Room, obj.Offering }
            );

        public async Task EditAsync(Meeting obj) =>
            await _db.ExecuteCheckedAsync(@"
                UPDATE meetings SET
                    day = @day,
                    time = @time,
                    duration = @duration,
                    room = @room,
                    offering = @offering
                WHERE id = @id",
                new {obj.Day, obj.Time, obj.Duration, obj.Room, obj.Offering, obj.Id}
            );

        public async Task DeleteAsync(Meeting obj) =>
            await _db.ExecuteCheckedAsync("DELETE FROM meetings WHERE id = @id", new {obj.Id});

        public async Task<IEnumerable<Meeting>> GetMeetingsForOfferingAsync(Offering offering) =>
            await _db.QueryAsync<Meeting>(@"
                SELECT m.id,
                       m.day,
                       m.time,
                       m.duration,
                       m.room,
                       m.offering
                FROM meetings m
                WHERE m.offering = @id",
                new {offering.Id}
            );
    }
}