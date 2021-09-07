using System;
using System.Collections.Generic;

namespace Elsa.Contracts
{
    public interface IActivityExecutionBuilder
    {
        IServiceProvider ServiceProvider { get; }
        IActivityExecutionBuilder Use(Func<ActivityMiddlewareDelegate, ActivityMiddlewareDelegate> middleware);
        public ActivityMiddlewareDelegate Build();
    }
}