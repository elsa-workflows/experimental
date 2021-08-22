using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IExpressionHandler
    {
        ValueTask<T> EvaluateAsync<T>(IExpression<T> expression, ActivityExecutionContext context);
    }
}