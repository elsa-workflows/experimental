using System;

namespace Elsa.Models
{
    public abstract class RegisterLocationReference
    {
        protected RegisterLocationReference() => Id = Guid.NewGuid().ToString("N");
        protected RegisterLocationReference(string id) => Id = id;

        public string Id { get; }

        public abstract RegisterLocation Declare();
        public object? Get(ActivityExecutionContext context) => context.Get(this);
        public T? Get<T>(ActivityExecutionContext context) => (T?)Get(context);
        public void Set(ActivityExecutionContext context, object? value) => context.Set(this, value);

        public RegisterLocation GetLocation(Register register) => register.TryGetLocation(Id, out var location) ? location : register.Declare(this);
    }
}