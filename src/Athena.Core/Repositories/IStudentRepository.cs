using System;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IStudentRepository : IRepository<Student, Guid>
    {
    }
}