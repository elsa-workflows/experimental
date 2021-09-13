using System;
using Elsa.Contracts;
using Elsa.Expressions;

namespace Elsa.Models
{
    public class Literal : RegisterLocationReference
    {
        public Literal()
        {
        }

        public Literal(object? value)
        {
            Value = value;
        }
        
        public object? Value { get; }
        public override RegisterLocation Declare() => new();
        public override RegisterLocation GetLocation(Register register) => register.TryGetLocation(Id, out var location) ? location : throw new InvalidOperationException("Literal does not exist");
    }

    public class Literal<T> : Literal
    {
        public Literal()
        {
        }

        public Literal(T value) : base(value!)
        {
        }

        public new T? Get(ActivityExecutionContext context) => (T?)base.Get(context);
    }
}