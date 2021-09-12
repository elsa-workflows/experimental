using System;
using Elsa.Contracts;
using Elsa.Expressions;
using Delegate = Elsa.Expressions.Delegate;

namespace Elsa.Models
{
    public abstract class Input : Argument
    {
        protected Input(IExpression expression) => Expression = expression;
        public IExpression Expression { get; }
    }


    public class Input<T> : Input
    {
        public Input(Variable<T> variable) : base(new VariableExpression(variable))
        {
        }

        public Input(Literal<T> literal) : base(literal)
        {
        }

        public Input(T literal) : this(new Literal<T>(literal))
        {
        }
        
        public Input(Delegate<T> @delegate) : base(@delegate)
        {
        }
        
        public Input(Delegate @delegate) : base(@delegate)
        {
        }
        
        public Input(Func<T?> func) : base(new Delegate<T>(func))
        {
        }

        public Input(IExpression expression) : base(expression)
        {
        }
    }
}