using System;

namespace Elsa.Models
{
    public abstract class RegisterLocationReference
    {
        protected RegisterLocationReference() => Id = Guid.NewGuid().ToString("N");
        protected RegisterLocationReference(string id) => Id = id;
        
        public string Id { get; }

        public abstract RegisterLocation Declare();
        public abstract RegisterLocation GetLocation(Register register);
        public object? Get(ActivityExecutionContext context) => context.Get(this);
        public void Set(ActivityExecutionContext context, object? value) => context.Set(this, value);
        
    }
}