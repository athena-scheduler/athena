using System;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IRepository<T, in TKey>
        where TKey : IEquatable<TKey>
        where T : class, IUniqueObject<TKey>
    {
        /// <summary>
        /// Get the object uniquely identified by the specified ID
        /// </summary>
        /// <param name="id">The ID of the object</param>
        /// <returns>The object identified by the specified id</returns>
        Task<T> GetAsync(TKey id);

        /// <summary>
        /// Adds the specified object to the repository
        /// </summary>
        /// <param name="obj">The object to add</param>
        Task AddAsync(T obj);
        
        /// <summary>
        /// Edit the specified object in the repository. The object must already exist
        /// </summary>
        /// <param name="obj">The object to edit</param>
        Task EditAsync(T obj);
        
        /// <summary>
        /// Remove the specified object from the repository
        /// </summary>
        /// <param name="obj">The object to remove</param>
        Task DeleteAsync(T obj);
    }
}