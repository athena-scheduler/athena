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
    public class InstitutionRepository : PostgresRepository, IInstitutionRepository
    {
        public InstitutionRepository(IDbConnection db) : base(db)
        {
        }

        public async Task<Institution> GetAsync(Guid id) =>
            (await _db.QueryAsync<Institution>("SELECT * FROM institutions WHERE id = @id", new {id}))
                .FirstOrDefault();

        public async Task AddAsync(Institution obj) => await _db.InsertUniqueAsync(
            "INSERT INTO institutions VALUES (@id, @name, @description)",
            new { obj.Id, obj.Name, obj.Description }
        );

        public async Task EditAsync(Institution obj) => await _db.ExecuteAsync(@"
            UPDATE institutions SET
                name = @name,
                description = @description
            WHERE id = @id",
            new { obj.Name, obj.Description, obj.Id }
        );

        public async Task DeleteAsync(Institution obj) => await _db.ExecuteAsync(
            "DELETE FROM institutions WHERE id = @id",
            new {obj.Id}
        );

        public async Task<IEnumerable<Institution>> SearchAsync(string query) => await _db.QueryAsync<Institution>(
            "SELECT * FROM institutions WHERE name ILIKE ('%' || @query || '%') OR description ILIKE ('%' || @query || '%')",
            new {query = query.Trim('%')}
        );

        public async Task<IEnumerable<Institution>> GetInstitutionsOnCampusAsync(Campus campus) =>
            await _db.QueryAsync<Institution>(@"
                SELECT i.id,
                       i.name,
                       i.description
                FROM institutions i
                    LEFT JOIN campus_x_institution link
                        ON i.id = link.institution
                WHERE link.campus = @id",
                new {campus.Id}
            );

        public async Task<IEnumerable<Institution>> GetInstitutionsForStudentAsync(Student student) =>
            await _db.QueryAsync<Institution>(@"
                SELECT i.id,
                       i.name,
                       i.description
                FROM institutions i
                    LEFT JOIN institution_x_student link
                        ON i.id = link.institution
                WHERE link.student = @id",
                new {student.Id}
            );

        public async Task EnrollStudentAsync(Institution institution, Student student) =>
            await _db.ExecuteAsync(
                "INSERT INTO institution_x_student VALUES (@institution, @student)",
                new { institution = institution.Id, student = student.Id }
            );

        public async Task UnenrollStudentAsync(Institution institution, Student student) =>
            await _db.ExecuteAsync(
                "DELETE FROM institution_x_student WHERE institution = @institution AND student = @student",
                new { institution = institution.Id, student = student.Id }
            );
    }
}