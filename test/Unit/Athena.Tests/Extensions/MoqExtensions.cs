using Moq;
using Moq.Language;
using Moq.Language.Flow;
using System.Threading.Tasks;

namespace Athena.Tests.Extensions
{
    public static class MoqExtensions
    {
        public static IReturnsResult<TMock> ReturnsNullAsync<TMock, TResult>(this IReturns<TMock, Task<TResult>> builder) where TMock: class => 
            builder.ReturnsAsync(default(TResult));
    }
}
