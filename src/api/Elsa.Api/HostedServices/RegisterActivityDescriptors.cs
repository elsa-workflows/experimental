using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Contracts;
using Microsoft.Extensions.Hosting;

namespace Elsa.Api.HostedServices
{
    public class RegisterActivityDescriptors : IHostedService
    {
        private readonly IActivityDescriptorRegistryPopulator _activityDescriptorRegistryPopulator;
        public RegisterActivityDescriptors(IActivityDescriptorRegistryPopulator activityDescriptorRegistryPopulator) => _activityDescriptorRegistryPopulator = activityDescriptorRegistryPopulator;
        public async Task StartAsync(CancellationToken cancellationToken) => await _activityDescriptorRegistryPopulator.PopulateRegistryAsync(cancellationToken);
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}