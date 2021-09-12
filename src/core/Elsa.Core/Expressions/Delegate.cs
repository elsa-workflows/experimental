using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Expressions
{
    public class Delegate : IExpression
    {
        public Delegate(Func<object?> value) => Value = value;
        public Func<object?> Value { get; }
    }

    public class Delegate<T> : Delegate
    {
        public Delegate(Func<T?> value) : base(() => value())
        {
        }
    }

    public class DelegateHandler : IExpressionHandler
    {
        public ValueTask<T?> EvaluateAsync<T>(IExpression input, ExpressionExecutionContext context)
        {
            var delegateExpression = (Delegate)input;
            var @delegate = delegateExpression.Value;
            var value = (T?)@delegate();
            return ValueTask.FromResult(value);
        }
    }
}