using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts;

public interface IExpressionEvaluator
{
    ValueTask<T?> EvaluateAsync<T>(IExpression input, ExpressionExecutionContext context);
    ValueTask<object?> EvaluateAsync(IExpression input, ExpressionExecutionContext context);
}