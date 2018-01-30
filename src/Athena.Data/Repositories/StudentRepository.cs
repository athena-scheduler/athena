using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Repositories;
using Athena.Data.Extensions;
using Dapper;

namespace Athena.Data.Repositories
{
    public class StudentRepository : PostgresRepository, IStudentRepository
    {
        public StudentRepository(IDbConnection db) : base(db)
        {
        }

        public async Task<Student> GetAsync(Guid id) =>
            (await _db.QueryAsync<Student>("SELECT * FROM students WHERE id = @id", new {id}))
                .FirstOrDefault();

        public async Task AddAsync(Student obj) =>
            await _db.InsertUniqueAsync(
                "INSERT INTO students VALUES (@id, @name, @email)",
                new {obj.Id, obj.Name, obj.Email}
            );

        public async Task EditAsync(Student obj) =>
            await _db.ExecuteAsync(
                "UPDATE students SET name = @name, email = @email WHERE id = @id",
                new {obj.Name, obj.Email, obj.Id}
            );

        public async Task DeleteAsync(Student obj) =>
            await _db.ExecuteAsync(
                "DELETE FROM students WHERE id = @id",
                new {obj.Id}
            );

        public async Task RegisterForProgramAsync(Student student, Program program) =>
            await _db.ExecuteAsync(
                "INSERT INTO student_x_program VALUES (@student, @program)",
                new {student = student.Id, program = program.Id}
            );

        public async Task UnregisterForProgramAsync(Student student, Program program) =>
            await _db.ExecuteAsync(
                "DELETE FROM student_x_program WHERE student = @student AND program = @program",
                new {student = student.Id, program = program.Id}
            );
    }
}