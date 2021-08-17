using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class NodeScheduler : INodeScheduler
    {
        private readonly Stack<ScheduledNode> _stack = new();

        public bool HasAny => _stack.Any();
        public void Schedule(ScheduledNode node) => _stack.Push(node);
        public ScheduledNode Unschedule() => _stack.Pop();
    }
}