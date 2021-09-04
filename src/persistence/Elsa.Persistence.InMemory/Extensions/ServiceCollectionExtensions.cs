using Elsa.Persistence.Abstractions.Contracts;
using Elsa.Persistence.InMemory.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Persistence.InMemory.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryWorkflowInstanceStore(this IServiceCollection services)
        {
            return services.AddSingleton<IWorkflowInstanceStore, InMemoryWorkflowInstanceStore>();
        }
        
        public static IServiceCollection AddInMemoryBookmarkStore(this IServiceCollection services)
        {
            return services.AddSingleton<IBookmarkStore, InMemoryBookmarkStore>();
        }
    }
}