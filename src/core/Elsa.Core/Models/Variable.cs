using System;
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
        public Variable()
        {
        }

        public Variable(T value) : base(value!)
        {
        }

        public new T? Get(ActivityExecutionContext context) => (T?)base.Get(context);
    }
}