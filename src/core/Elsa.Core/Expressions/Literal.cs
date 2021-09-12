using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Expressions
{
    public class Literal : IExpression
    {
        public Literal(object? value) => Value = value;
        public object? Value { get; }
    }

    public class Literal<T> : Literal
    {
        public Literal(T? value) : base(value)
        {
        }
    }

    public class LiteralHandler : IExpressionHandler
    {
        public ValueTask<T?> EvaluateAsync<T>(IExpression input, ExpressionExecutionContext context)
        {
            var literal = (Literal)input;
            var value = (T?)literal.Value;
            return ValueTask.FromResult(value);
        }
    }
}