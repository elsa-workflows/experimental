using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IExpressionHandler
    {
        ValueTask<object?> EvaluateAsync(IExpression expression, ExpressionExecutionContext context);
    }
}