using System;
using System.Threading.Tasks;

namespace Elsa.Models
{
    public class DelegateReference : RegisterLocationReference
    {
        public DelegateReference()
        {
        }

        public DelegateReference(Func<object?> @delegate) => Delegate = _ => ValueTask.FromResult(@delegate());
        public DelegateReference(Func<ActivityExecutionContext, object?> @delegate) => Delegate = x => ValueTask.FromResult(@delegate(x));
        public DelegateReference(Func<ActivityExecutionContext, ValueTask<object?>> @delegate) => Delegate = @delegate;

        public Func<ActivityExecutionContext, ValueTask<object?>>? Delegate { get; set; }
        public override RegisterLocation Declare() => new();
        public override RegisterLocation GetLocation(Register register) => register.TryGetLocation(Id, out var location) ? location : throw new InvalidOperationException("Delegate does not exist");
    }

    public class DelegateReference<T> : DelegateReference
    {
        public DelegateReference()
        {
        }

        public DelegateReference(Func<T?> @delegate) : base(x => @delegate())
        {
        }
        
        public DelegateReference(Func<ActivityExecutionContext, T?> @delegate) : base(x => @delegate(x))
        {
        }
        
        public DelegateReference(Func<ActivityExecutionContext, ValueTask<T?>> @delegate) : base(x => @delegate(x))
        {
        }
    }
}