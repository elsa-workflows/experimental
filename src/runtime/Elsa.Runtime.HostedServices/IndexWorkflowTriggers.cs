using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Persistence.Abstractions.Models;
using Elsa.Runtime.Contracts;
using Elsa.Runtime.Models;
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
            const int limit = 100;
            PagedList<Workflow> pagedList;
            var stopwatch = new Stopwatch();
            var currentBatch = 1;

            _logger.LogInformation("Indexing workflow triggers in batches of {Limit}", limit);
            stopwatch.Start();

            do
            {
                pagedList = await workflowRegistry.ListAsync(new PagerParameters(limit), cancellationToken);
                var indexTasks = pagedList.Items.Select(x => triggerIndexer.IndexTriggersAsync(x, cancellationToken));
                await Task.WhenAll(indexTasks);
                _logger.LogInformation("Indexed batch {CurrentBatch}", currentBatch++);
            } while (pagedList.Cursor != null);

            stopwatch.Stop();
            _logger.LogInformation("Finished indexing workflow triggers in {ElapsedTime}", stopwatch.Elapsed);
        }
    }
}