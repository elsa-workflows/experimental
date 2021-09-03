using System;
using System.Collections.Generic;

namespace Elsa.Contracts
{
    public interface IActivityExecutionBuilder
    {
        public IDictionary<string, object?> Properties { get; }
        IServiceProvider ApplicationServices { get; }
        IActivityExecutionBuilder Use(Func<ActivityMiddlewareDelegate, ActivityMiddlewareDelegate> middleware);
        public ActivityMiddlewareDelegate Build();
    }
}