using System.Threading.Tasks;

namespace Athena.Core.Models.Identity
{
    public interface IUserApiKeyStore
    {
        /// <summary>
        /// Gets the user associated with the specified API key
        /// </summary>
        /// <param name="key">The API Key to look for</param>
        /// <returns>A user associated with the specified API Key, or null if not found</returns>
        Task<AthenaUser> GetUserForApiKey(string key);
    }
}