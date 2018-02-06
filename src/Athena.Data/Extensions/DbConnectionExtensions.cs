using System.Data;
using System.Threading.Tasks;
using Athena.Core.Exceptions;
using Dapper;
using Npgsql;

namespace Athena.Data.Extensions
{
    public static class DbConnectionExtensions
    {
        public static async Task InsertUniqueAsync(this IDbConnection db, string sql, object param)
        {
            try
            {
                await db.ExecuteAsync(sql, param);
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "23505")
                {
                    throw new DuplicateObjectException(sql, param);
                }

                throw;
            }
        }
    }
}