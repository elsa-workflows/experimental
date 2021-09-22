using System;
using Elsa.Contracts;
using Elsa.Expressions;

namespace Elsa.Models
{
    public abstract class Input : Argument
    {
        protected Input(IExpression expression, RegisterLocationReference locationReference, Type targetType) : base(locationReference)
        {
            Expression = expression;
            TargetType = targetType;
        }

        public IExpression Expression { get; }
        public Type TargetType { get; set; }
    }


    public class Input<T> : Input
    {
        public Input(T literal) : this(new Literal<T>(literal))
        {
        }

        public Input(Func<T> @delegate) : this(new DelegateReference(() => @delegate()))
        {
        }
        
        public Input(Func<ExpressionExecutionContext, T> @delegate) : this(new DelegateReference<T>(@delegate))
        {
        }
        
        public Input(Variable<T> variable) : base(new VariableExpression(variable), variable, typeof(T))
        {
        }

        public Input(Literal<T> literal) : base(new LiteralExpression(literal.Value), literal, typeof(T))
        {
        }

        public Input(DelegateReference delegateReference) : base(new DelegateExpression(delegateReference), delegateReference, typeof(T))
        {
        }
    }
}