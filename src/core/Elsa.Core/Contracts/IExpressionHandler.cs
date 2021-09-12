using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IExpressionHandler
    {
        ValueTask<T?> EvaluateAsync<T>(IExpression input, ExpressionExecutionContext context);
    }
}