using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IInstitutionRepository : IRepository<Institution, Guid>
    {
    }
}