using System;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IRepository<T, in TKey>
        where TKey : IEquatable<TKey>
        where T : class, IUniqueObject<TKey>
    {
        Task<T> GetAsync(TKey id);

        Task<T> AddAsync(T obj);
        Task<T> EditAsync(T obj);
        Task<T> DeleteAsync(T obj);
    }
}