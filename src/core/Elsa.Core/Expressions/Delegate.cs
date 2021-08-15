using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Contracts;

namespace Elsa.Expressions
{
    public class Delegate<T> : IExpression<T>
    {
        public Delegate(Func<T> value) => Value = value;
        public Func<T> Value { get; }   
    }

    public class DelegateHandler : IExpressionHandler
    {
        public ValueTask<T> EvaluateAsync<T>(IExpression<T> expression, CancellationToken cancellationToken = default)
        {
            var delegateExpression = (Delegate<T>)expression;
            var value = delegateExpression.Value();
            return new ValueTask<T>(value);
        }
    }
}