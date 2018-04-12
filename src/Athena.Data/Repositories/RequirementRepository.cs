using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Repositories;
using Athena.Data.Extensions;
using Dapper;

namespace Athena.Data.Repositories
{
    public class RequirementRepository : PostgresRepository, IRequirementRepository
    {
        public RequirementRepository(IDbConnection db) : base(db)
        {
        }

        public async Task<Requirement> GetAsync(Guid id) =>
            (await _db.QueryAsync<Requirement>("SELECT * FROM requirements WHERE id = @id", new {id}))
                .FirstOrDefault();

        public async Task AddAsync(Requirement obj) =>
            await _db.ExecuteCheckedAsync(
                "INSERT INTO requirements VALUES (@id, @name, @description)",
                new {obj.Id, obj.Name, obj.Description}
            );

        public async Task EditAsync(Requirement obj) =>
            await _db.ExecuteCheckedAsync(
                "UPDATE requirements SET name = @name, description = @description WHERE id = @id",
                new {obj.Name, obj.Description, obj.Id}
            );

        public async Task DeleteAsync(Requirement obj) =>
            await _db.ExecuteCheckedAsync(
                "DELETE FROM requirements WHERE id = @id",
                new {obj.Id}
            );

        public async Task<IEnumerable<Requirement>> GetRequirementsCourseSatisfiesAsync(Course course) =>
            await _db.QueryAsync<Requirement>(@"
                SELECT r.id,
                       r.name,
                       r.description
                FROM requirements r
                    LEFT JOIN course_requirements link
                        ON r.id = link.requirement
                WHERE link.course = @id",
                new {course.Id}
            );

        public async Task<IEnumerable<Requirement>> GetPrereqsForCourseAsync(Course course) =>
            await _db.QueryAsync<Requirement>(@"
                SELECT r.id,
                       r.name,
                       r.description
                FROM requirements r
                    LEFT JOIN course_prereqs link
                        ON r.id = link.prereq
                WHERE link.course = @id",
                new {course.Id}
            );
        
        public async Task<IEnumerable<Requirement>> GetConcurrentPrereqsAsync(Course course) =>
            await _db.QueryAsync<Requirement>(@"
                SELECT r.id,
                       r.name,
                       r.description
                FROM requirements r
                    LEFT JOIN course_concurrent_prereqs link
                        ON r.id = link.prereq
                WHERE link.course = @id",
                new {course.Id}
            );

        public async Task<IEnumerable<Requirement>> GetRequirementsForProgramAsync(Program program) =>
            await _db.QueryAsync<Requirement>(@"
                SELECT r.id,
                       r.name,
                       r.description
                FROM requirements r
                    LEFT JOIN program_requirements link
                        ON r.id = link.requirement
                WHERE link.program = @id",
                new {program.Id}
            );

        public async Task<IEnumerable<Requirement>> GetInProgressRequirementsForStudentAsync(Student student) =>
            await _db.QueryAsync<Requirement>(@"
                SELECT r.id,
                       r.name,
                       r.description
                FROM requirements r
                    LEFT JOIN course_requirements c
                        ON r.id = c.requirement
                    LEFT JOIN student_x_in_progress_course link
                        ON c.course = link.course
                WHERE link.student = @id",
                new {student.Id}
            );

        public async Task<IEnumerable<Requirement>> GetCompletedRequirementsForStudentAsync(Student student) =>
            await _db.QueryAsync<Requirement>(@"
                SELECT r.id,
                       r.name,
                       r.description
                FROM requirements r
                    LEFT JOIN course_requirements c
                        ON r.id = c.requirement
                    LEFT JOIN student_x_completed_course link
                        ON c.course = link.course
                WHERE link.student = @id",
                new {student.Id}
            );
    }
}