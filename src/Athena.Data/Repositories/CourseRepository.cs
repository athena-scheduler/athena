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
    public class CourseRepository : PostgresRepository, ICourseRepository
    {
        public CourseRepository(IDbConnection db) : base(db)
        {
        }

        public async Task<Course> GetAsync(Guid id) =>
            (await _db.QueryAsync<Course, Institution, Course>(@"
                SELECT c.id,
                       c.name,
                       i.id,
                       i.name,
                       i.description
                FROM courses c
                    LEFT JOIN institutions i
                        ON c.institution = i.id
                WHERE c.id = @id",
                _mapInstitution,
                new {id}
            )).FirstOrDefault();

        public async Task AddAsync(Course obj) => await _db.InsertCheckedAsync(
            "INSERT INTO courses VALUES (@id, @name, @institution)",
            new {obj.Id, obj.Name, institution = obj.Institution.Id}
        );

        public async Task EditAsync(Course obj) => await _db.ExecuteAsync(@"
            UPDATE courses SET
                name = @name,
                institution = @institution
            WHERE id = @id",
            new {obj.Name, institution = obj.Institution.Id, obj.Id}
        );

        public async Task DeleteAsync(Course obj) => await _db.ExecuteAsync(
            "DELETE FROM courses WHERE id = @id",
            new {obj.Id}
        );

        public async Task<IEnumerable<Course>> GetCoursesForInstitutionAsync(Institution institution) =>
            await _db.QueryAsync<Course, Institution, Course>(@"
                SELECT c.id,
                       c.name,
                       i.id,
                       i.name,
                       i.description
                FROM courses c
                    LEFT JOIN institutions i
                        ON c.institution = i.id
                WHERE c.institution = @id",
                _mapInstitution,
                new {institution.Id}
            );

        public async Task<IEnumerable<Course>> GetCompletedCoursesForStudentAsync(Student student) =>
            await _db.QueryAsync<Course, Institution, Course>(@"
                SELECT c.id,
                       c.name,
                       i.id,
                       i.name,
                       i.description
                FROM courses c
                    LEFT JOIN institutions i
                        ON c.institution = i.id
                    LEFT JOIN student_x_completed_course link
                        ON c.id = link.course
                WHERE link.student = @student",
                _mapInstitution,
                new { student = student.Id }
            );

        public async Task MarkCourseAsCompletedForStudentAsync(Course course, Student student) =>
            await _db.InsertCheckedAsync(
                "INSERT INTO student_x_completed_course VALUES (@student, @course)",
                new {student = student.Id, course = course.Id}
            );

        public async Task MarkCourseAsUncompletedForStudentAsync(Course course, Student student) =>
            await _db.ExecuteAsync(
                "DELETE FROM student_x_completed_course WHERE student = @student AND course = @course",
                new {student = student.Id, course = course.Id}
            );

        public async Task AddSatisfiedRequirementAsync(Course course, Requirement requirement) =>
            await _db.InsertCheckedAsync(
                "INSERT INTO course_requirements VALUES (@course, @requirement)",
                new {course = course.Id, requirement = requirement.Id}
            );

        public async Task RemoveSatisfiedRequirementAsync(Course course, Requirement requirement) =>
            await _db.ExecuteAsync(
                "DELETE FROM course_requirements WHERE course = @course AND requirement = @requirement",
                new {course = course.Id, requirement = requirement.Id}
            );

        public async Task AddPrerequisiteAsync(Course course, Requirement prereq) =>
            await _db.InsertCheckedAsync(
                "INSERT INTO course_prereqs VALUES (@course, @prereq)",
                new {course = course.Id, prereq = prereq.Id}
            );

        public async Task RemovePrerequisiteAsync(Course course, Requirement prereq) =>
            await _db.ExecuteAsync(
                "DELETE FROM course_prereqs WHERE course = @course AND prereq = @prereq",
                new {course = course.Id, prereq = prereq.Id}
            );
        
        public async Task AddConcurrentPrerequisiteAsync(Course course, Requirement prereq) =>
            await _db.InsertCheckedAsync(
                "INSERT INTO course_concurrent_prereqs VALUES (@course, @prereq)",
                new {course = course.Id, prereq = prereq.Id}
            );
        
        public async Task RemoveConcurrentPrerequisiteAsync(Course course, Requirement prereq) =>
            await _db.ExecuteAsync(
                "DELETE FROM course_concurrent_prereqs WHERE course = @course AND prereq = @prereq",
                new {course = course.Id, prereq = prereq.Id}
            );

        public async Task<IEnumerable<Course>> SearchAsync(CourseSearchOptions query)
        {
            if (query.StudentId == Guid.Empty)
            {
                throw new ArgumentException("Student Id required", nameof(query.StudentId));
            }

            return await (query.Completed ? searchCompletedAsync(query) : searchNonCompletedAsync(query));
        }

        private async Task<IEnumerable<Course>> searchNonCompletedAsync(CourseSearchOptions query) =>
            await _db.QueryAsync<Course, Institution, Course>(@"
                SELECT c.id,
                       c.name,
                       i.id,
                       i.name,
                       i.description
                FROM courses c
                    LEFT JOIN institutions i
                        ON c.institution = i.id
                WHERE c.id NOT IN (SELECT course FROM student_x_completed_course WHERE student = @student)
                      AND c.name ILIKE ('%' || @q || '%')
                      AND i.id IN (SELECT institution FROM institution_x_student WHERE student = @student)",
                _mapInstitution,
                new { student = query.StudentId, q = query.Query }
            );

        private async Task<IEnumerable<Course>> searchCompletedAsync(CourseSearchOptions query) =>
            await _db.QueryAsync<Course, Institution, Course>(@"
                SELECT c.id,
                       c.name,
                       i.id,
                       i.name,
                       i.description
                FROM courses c
                    LEFT JOIN institutions i
                        ON c.institution = i.id
                    LEFT JOIN student_x_completed_course link
                        ON c.id = link.course
                WHERE link.student = @student AND c.name ILIKE ('%' || @q || '%')",
                _mapInstitution,
                new { student = query.StudentId, q = query.Query }
            );

        private static Course _mapInstitution(Course c, Institution i)
        {
            c.Institution = i;
            return c;
        }
    }
}