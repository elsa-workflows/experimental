namespace Elsa.Contracts
{
    public interface INodeSchedulerFactory
    {
        INodeScheduler CreateScheduler();
    }
}