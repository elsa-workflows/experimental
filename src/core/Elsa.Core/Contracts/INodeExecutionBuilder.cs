using System;
using System.Collections.Generic;

namespace Elsa.Contracts
{
    public interface INodeExecutionBuilder
    {
        public IDictionary<string, object?> Properties { get; }
        IServiceProvider ApplicationServices { get; }
        INodeExecutionBuilder Use(Func<ExecuteNode, ExecuteNode> middleware);
        public ExecuteNode Build();
    }
}