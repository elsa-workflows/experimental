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

        public Variable(object? defaultValue) : this(new Literal(defaultValue))
        {
        }

        public Variable(IExpression defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public string? Name { get; set; }
        public IExpression DefaultValue { get; } = new Literal(null);
        public override RegisterLocation Declare() => new();
        public override RegisterLocation GetLocation(Register register) => register.TryGetLocation(Id, out var location) ? location : throw new InvalidOperationException("Variable does not exist");
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