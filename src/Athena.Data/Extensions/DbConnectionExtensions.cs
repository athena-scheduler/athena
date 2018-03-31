using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Transactions;
using Athena.Core.Exceptions;
using Dapper;
using Npgsql;

namespace Athena.Data.Extensions
{
    public static class DbConnectionExtensions
    {
        public static async Task<int> InsertUniqueAsync(this IDbConnection db, string sql, object param)
        {
            try
            {
                return await db.ExecuteAsync(sql, param);
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
        
        /// <summary>
        /// Create a transcation scope with Async Support and enlist the specified connection with the transaction
        /// </summary>
        /// <param name="db">The DB Connection to enlist</param>
        /// <returns>A transaction scope</returns>
        public static TransactionScope CreateAsyncTransactionScope(this IDbConnection db)
        {
            var result = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (db.State != ConnectionState.Open)
            {
                db.Open();
            }
            
            (db as DbConnection)?.EnlistTransaction(Transaction.Current);

            return result;
        }
    }
}