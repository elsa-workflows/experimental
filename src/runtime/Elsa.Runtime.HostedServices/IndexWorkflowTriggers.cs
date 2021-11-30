using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Runtime.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elsa.Runtime.HostedServices
{
    public class IndexWorkflowTriggers : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public IndexWorkflowTriggers(IServiceScopeFactory serviceScopeFactory, ILogger<IndexWorkflowTriggers> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var triggerIndexer = scope.ServiceProvider.GetRequiredService<ITriggerIndexer>();
            var workflowManager = scope.ServiceProvider.GetRequiredService<IWorkflowRegistry>();
            await IndexAsync(triggerIndexer, workflowManager, cancellationToken);
        }

        private async Task IndexAsync(ITriggerIndexer triggerIndexer, IWorkflowRegistry workflowRegistry, CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();

            _logger.LogInformation("Indexing workflow triggers");
            stopwatch.Start();

            var workflows = workflowRegistry.StreamAllAsync(cancellationToken);

            await foreach (var workflow in workflows.WithCancellation(cancellationToken))
                await triggerIndexer.IndexTriggersAsync(workflow, cancellationToken);

            stopwatch.Stop();
            _logger.LogInformation("Finished indexing workflow triggers in {ElapsedTime}", stopwatch.Elapsed);
        }
    }
}