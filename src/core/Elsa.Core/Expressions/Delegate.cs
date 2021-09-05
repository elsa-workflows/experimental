using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Expressions
{
    public class Delegate<T> : IExpression<T>
    {
        public Delegate(Func<ExpressionExecutionContext, T> value) => Value = value;
        public Delegate(Func<T> value) => Value = _ => value();
        public Func<ExpressionExecutionContext, T> Value { get; }   
    }

    public class DelegateHandler : IExpressionHandler
    {
        public ValueTask<T> EvaluateAsync<T>(IExpression<T> expression, ExpressionExecutionContext context)
        {
            var delegateExpression = (Delegate<T>)expression;
            var value = delegateExpression.Value(context);
            return new ValueTask<T>(value);
        }
    }
}