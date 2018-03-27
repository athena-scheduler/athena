using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface ISearchableRepository<T, in TKey, in TSearchOptions> : IRepository<T, TKey>
        where TKey : IEquatable<TKey>
        where T : class, IUniqueObject<TKey>
    {
        Task<IEnumerable<T>> SearchAsync(TSearchOptions query);
    }
}