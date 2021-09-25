using Elsa.Contracts;
using Elsa.Expressions;

namespace Elsa.Models
{
    public class Variable : RegisterLocationReference
    {
        public Variable()
        {
        }

        public Variable(object? defaultValue) : this(new LiteralExpression(defaultValue))
        {
        }

        public Variable(IExpression defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public string? Name { get; set; }
        public IExpression? DefaultValue { get; }
        public override RegisterLocation Declare() => new();
    }

    public class Variable<T> : Variable
    {
        public Variable() : base(default(T))
        {
        }

        public Variable(T value) : base(value ?? default)
        {
        }
        
        public new T? Get(ExpressionExecutionContext context) => (T?)base.Get(context);
    }
}