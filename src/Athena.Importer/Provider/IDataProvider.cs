using System;
using System.Collections.Generic;
using Athena.Core.Models;

namespace Athena.Importer.Provider
{
    public interface IDataProvider<out T> where T : IUniqueObject<Guid>
    {
        IEnumerable<T> GetData();
    }
}