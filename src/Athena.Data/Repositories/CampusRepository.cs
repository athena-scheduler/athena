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
    public class CampusRepository : PostgresRepository, ICampusRepository
    {
        public CampusRepository(IDbConnection db) : base(db)
        {
        }

        public async Task<Campus> GetAsync(Guid id) =>
            (await _db.QueryAsync<Campus>("SELECT * FROM campuses WHERE id = @id", new {id}))
                .FirstOrDefault();

        public async Task AddAsync(Campus obj) => await _db.InsertCheckedAsync(
            "INSERT INTO campuses VALUES (@id, @name, @description, @location)",
            new {obj.Id, obj.Name, obj.Description, obj.Location}
        );

        public async Task EditAsync(Campus obj) => await _db.ExecuteAsync(@"
            UPDATE campuses SET
                name = @name,
                description = @description,
                location = @location
            WHERE id = @id",
            new { obj.Name, obj.Description, obj.Location, obj.Id }
        );

        public async Task DeleteAsync(Campus obj) => await _db.ExecuteAsync(
            "DELETE FROM campuses WHERE id = @id",
            new {obj.Id}
        );

        public async Task<IEnumerable<Campus>> GetCampusesForInstitutionAsync(Institution institution) =>
            await _db.QueryAsync<Campus>(@"
                SELECT c.id,
                       c.name,
                       c.description,
                       c.location
                FROM campuses c
                    LEFT JOIN campus_x_institution link
                        ON c.id = link.campus
                WHERE link.institution = @id",
                new {institution.Id}
            );

        public async Task AssociateCampusWithInstitutionAsync(Campus campus, Institution institution) =>
            await _db.InsertCheckedAsync(
                "INSERT INTO campus_x_institution VALUES (@campus, @institution)",
                new {campus = campus.Id, institution = institution.Id}
            );

        public async Task DissassociateCampusWithInstitutionAsync(Campus campus, Institution institution) =>
            await _db.ExecuteAsync(
                "DELETE FROM campus_x_institution WHERE campus = @campus AND institution = @institution",
                new {campus = campus.Id, institution = institution.Id}
            );
    }
}