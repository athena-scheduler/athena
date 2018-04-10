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
    public class ProgramRepository : PostgresRepository, IProgramRepository
    {
        public ProgramRepository(IDbConnection db) : base(db)
        {
        }

        public async Task<Program> GetAsync(Guid id) =>
            (await _db.QueryAsync<Program, Institution, Program>(@"
                SELECT p.id,
                       p.name,
                       p.description,
                       i.id,
                       i.name,
                       i.description
                FROM programs p
                    LEFT JOIN institutions i
                        ON p.institution = i.id
                WHERE p.id = @id",
                _mapProgram,
                new {id}
            )).FirstOrDefault();

        public async Task AddAsync(Program obj) => await _db.ExecuteCheckedAsync(
            "INSERT INTO programs VALUES (@id, @name, @description, @institution)",
            new {obj.Id, obj.Name, obj.Description, institution = obj.Institution.Id}
        );

        public async Task EditAsync(Program obj) => await _db.ExecuteCheckedAsync(
            "UPDATE programs SET name = @name, description = @description, institution = @institution WHERE id = @id",
            new {obj.Name, obj.Description, institution = obj.Institution.Id, obj.Id}
        );

        public async Task DeleteAsync(Program obj) => await _db.ExecuteCheckedAsync(
            "DELETE FROM programs WHERE id = @id",
            new {obj.Id}
        );

        public async Task<IEnumerable<Program>> SearchAsync(ProgramSearchOptions query)
        {
            var sql = @"
                SELECT p.id,
                       p.name,
                       p.description,
                       i.id,
                       i.name,
                       i.description
                FROM programs p
                    LEFT JOIN institutions i
                        ON p.institution = i.id
                WHERE (p.name ILIKE ('%' || @query || '%') or p.description ILIKE ('%' || @query || '%'))";

            object opts = new {query = query.Query};

            var iid = query.InstitutionIds?.ToList();
            if (iid?.Any() ?? false)
            {
                sql += " AND i.id = ANY (@iid)";
                opts = new {query = query.Query, iid};
            }
            
            return await _db.QueryAsync<Program, Institution, Program>(
                sql,
                _mapProgram,
                opts
            );
        }

        public async Task<IEnumerable<Program>> GetProgramsOfferedByInstitutionAsync(Institution institution) =>
            await _db.QueryAsync<Program, Institution, Program>(@"
                SELECT p.id,
                       p.name,
                       p.description,
                       i.id,
                       i.name,
                       i.description
                FROM programs p
                    LEFT JOIN institutions i
                        ON p.institution = i.id
                WHERE p.institution = @id",
                _mapProgram,
                new {institution.Id}
            );

        public async Task<IEnumerable<Program>> GetProgramsForStudentAsync(Student student) =>
            await _db.QueryAsync<Program, Institution, Program>(@"
                SELECT p.id,
                       p.name,
                       p.description,
                       i.id,
                       i.name,
                       i.description
                FROM programs p
                    LEFT JOIN institutions i
                        ON p.institution = i.id
                    LEFT JOIN student_x_program link
                        ON p.id = link.program
                WHERE link.student = @id",
                _mapProgram,
                new {student.Id}
            );

        public async Task AddRequirementAsync(Program program, Requirement requirement) =>
            await _db.ExecuteCheckedAsync(
                "INSERT INTO program_requirements VALUES (@program, @req)",
                new {program = program.Id, req = requirement.Id}
            );

        public async Task RemoveRequirementAsync(Program program, Requirement requirement) =>
            await _db.ExecuteCheckedAsync(
                "DELETE FROM program_requirements WHERE program = @program AND requirement = @req",
                new {program = program.Id, req = requirement.Id}
            );

        private static Program _mapProgram(Program p, Institution i)
        {
            p.Institution = i;
            return p;
        }
    }
}