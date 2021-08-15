using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Contracts
{
    public interface IExpressionHandler
    {
        ValueTask<T> EvaluateAsync<T>(IExpression<T> expression, CancellationToken cancellationToken = default);
    }
}