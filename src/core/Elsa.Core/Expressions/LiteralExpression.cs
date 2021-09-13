using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Expressions
{
    public class LiteralExpression : IExpression
    {
        public LiteralExpression(object? value) => Value = value;
        public object? Value { get; }
    }

    public class LiteralExpression<T> : LiteralExpression
    {
        public LiteralExpression(T? value) : base(value)
        {
        }
    }

    public class LiteralHandler : IExpressionHandler
    {
        public ValueTask<object?> EvaluateAsync(IExpression input, ExpressionExecutionContext context)
        {
            var variableReference = (LiteralExpression)input;
            var value = variableReference.Value;
            return ValueTask.FromResult(value);
        }
    }
}