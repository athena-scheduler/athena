using System.Data;
using System.Data.Common;
using System.Transactions;

namespace Athena.Data
{
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Create a transcation scope with Async Support and enlist the specified connection with the transaction
        /// </summary>
        /// <param name="db">The DB Connection to enlist</param>
        /// <returns>A transaction scope</returns>
        public static TransactionScope CreateAsyncTransactionScope(this IDbConnection db)
        {
            var result = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            (db as DbConnection)?.EnlistTransaction(Transaction.Current);

            return result;
        }
    }
}