using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Expressions
{
    public class Literal<T> : IExpression<T>
    {
        public Literal(T value) => Value = value;
        public T Value { get; }   
    }
    
    public class LiteralHandler : IExpressionHandler
    {
        public ValueTask<T> EvaluateAsync<T>(IExpression<T> expression, NodeExecutionContext context)
        {
            var literalExpression = (Literal<T>)expression;
            var value = literalExpression.Value;
            return new ValueTask<T>(value);
        }
    }
}