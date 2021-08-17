using Elsa.Contracts;

namespace Elsa.Services
{
    public class NodeSchedulerFactory : INodeSchedulerFactory
    {
        public INodeScheduler CreateScheduler() => new NodeScheduler();
    }
}