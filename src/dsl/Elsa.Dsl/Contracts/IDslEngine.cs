using Elsa.Models;

namespace Elsa.Dsl.Contracts
{
    public interface IDslEngine
    {
        WorkflowDefinition Parse(string script);
    }
}