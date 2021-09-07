using Elsa.Runtime.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DefaultWorkflowEngineBuilder
    {
        public static WorkflowEngineBuilder CreateDefaultBuilder()
        {
            var builder = new WorkflowEngineBuilder();

            builder.Services.AddElsa();

            return builder;
        }
    }
}