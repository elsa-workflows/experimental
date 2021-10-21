using Elsa.Contracts;

namespace Elsa.Activities.Containers.FreeFlowchart
{
    public record Connection(IActivity Source, IActivity Target);
}