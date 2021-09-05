using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IExpressionEvaluator
    {
        ValueTask<T> EvaluateAsync<T>(IExpression<T> expression, ExpressionExecutionContext context);
    }
}