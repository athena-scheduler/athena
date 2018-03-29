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
        private readonly IMeetingRepository _meetings;

        public OfferingRepository(IDbConnection db, IMeetingRepository meetings) : base(db) =>
            _meetings = meetings ?? throw new ArgumentNullException(nameof(meetings));

        public async Task<Offering> GetAsync(Guid id)
        {
            using (var scope = _db.CreateAsyncTransactionScope())
            {
                var result = (await _db.QueryAsync<Offering, Course, Institution, Campus, Offering>(@"
                     SELECT o.id,
                            o.start,
                            o.end,
                            co.id,
                            co.name,
                            i.id,
                            i.name,
                            i.description,
                            ca.id,
                            ca.name,
                            ca.description,
                            ca.location
                     FROM offerings o
                        LEFT JOIN courses co
                            ON o.course = co.id
                        LEFT JOIN institutions i
                            ON co.institution = i.id
                        LEFT JOIN campuses ca
                            ON o.campus = ca.id
                     WHERE o.id = @id",
                    _mapOffering,
                    new { id }
                )).FirstOrDefault();
    
                if (result != null)
                {
                    result.Meetings = await _meetings.GetMeetingsForOfferingAsync(result);
                }
    
                scope.Complete();
                return result;
            }
        }
            
        public async Task AddAsync(Offering obj) =>
            await _db.InsertUniqueAsync(
                "INSERT INTO offerings VALUES (@id, @campus, @start, @end, @course)",
                new {obj.Id, campus = obj.Campus.Id, obj.Start, obj.End, course = obj.Course.Id}
            );
        
        public async Task EditAsync(Offering obj) =>
            await _db.ExecuteAsync(
                "UPDATE offerings SET campus = @campus, start = @start, \"end\" = @end, course = @course WHERE id = @id",
                new { campus = obj.Campus.Id, obj.Start, obj.End, course = obj.Course.Id, obj.Id }
            );

        public async Task DeleteAsync(Offering obj) =>
            await _db.ExecuteAsync(
                "DELETE FROM offerings WHERE id = @id",
                new {id = obj.Id}
            );

        public async Task<IEnumerable<Offering>> GetOfferingsForCourseAsync(Course course)
        {
            using (var scope = _db.CreateAsyncTransactionScope())
            {
                var results = (await _db.QueryAsync<Offering, Course, Institution, Campus, Offering>(@"
                SELECT o.id,
                       o.start,
                       o.end,
                       co.id,
                       co.name,
                       i.id,
                       i.name,
                       i.description,
                       ca.id,
                       ca.name,
                       ca.description,
                       ca.location
                FROM offerings o
                    LEFT JOIN courses co
                        ON o.course = co.id
                    LEFT JOIN institutions i
                        ON co.institution = i.id
                    LEFT JOIN campuses ca
                        ON o.campus = ca.id
                WHERE o.course = @id",
                    _mapOffering,
                    new {course.Id}
                )).ToList();

                foreach (var offering in results)
                {
                    offering.Meetings = await _meetings.GetMeetingsForOfferingAsync(offering);
                }

                scope.Complete();
                return results;
            }
        }

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

        public async Task<IEnumerable<Offering>> GetInProgressOfferingsForStudent(Student student)
        {
            using (var scope = _db.CreateAsyncTransactionScope())
            {
                var results = (await _db.QueryAsync<Offering, Course, Institution, Campus, Offering>(@"
                SELECT o.id,
                       o.start,
                       o.end,
                       co.id,
                       co.name,
                       i.id,
                       i.name,
                       i.description,
                       ca.id,
                       ca.name,
                       ca.description,
                       ca.location
                FROM offerings o
                    LEFT JOIN courses co
                        ON o.course = co.id
                    LEFT JOIN institutions i
                        ON co.institution = i.id
                    LEFT JOIN campuses ca
                        ON o.campus = ca.id
                    LEFT JOIN student_x_in_progress_course link
                        ON o.id = link.offering
                WHERE link.student = @id",
                    _mapOffering,
                    new {student.Id}
                )).ToList();

                foreach (var offering in results)
                {
                    offering.Meetings = await _meetings.GetMeetingsForOfferingAsync(offering);
                }

                scope.Complete();
                return results;
            }
        }

        public async Task EnrollStudentInOffering(Student student, Offering offering) =>
            await _db.InsertUniqueAsync(
                "INSERT INTO student_x_in_progress_course VALUES (@student, @course, @offering)", 
                new { student = student.Id, course = offering.Course.Id, offering = offering.Id }
            );

        public async Task UnenrollStudentInOffering(Student student, Offering offering) =>
            await _db.InsertUniqueAsync(
                "DELETE FROM student_x_in_progress_course WHERE student = @student AND course = @course AND offering = @offering", 
                new { student = student.Id, course = offering.Course.Id, offering = offering.Id }
            );

        private static Offering _mapOffering(Offering o, Course co, Institution i, Campus ca)
        {
            o.Course = co;
            co.Institution = i;
            o.Campus = ca;
            return o;
        }
    }
}