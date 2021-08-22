using System;
using System.Collections.Generic;

namespace Elsa.Contracts
{
    public interface INodeExecutionBuilder
    {
        public IDictionary<string, object?> Properties { get; }
        IServiceProvider ApplicationServices { get; }
        INodeExecutionBuilder Use(Func<ActivityMiddlewareDelegate, ActivityMiddlewareDelegate> middleware);
        public ActivityMiddlewareDelegate Build();
    }
}