using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Contracts;
using Microsoft.Extensions.Hosting;

namespace Elsa.Api.HostedServices;

public class RegisterActivityDescriptors : IHostedService
{
    private readonly IActivityRegistryPopulator _activityRegistryPopulator;
    public RegisterActivityDescriptors(IActivityRegistryPopulator activityRegistryPopulator) => _activityRegistryPopulator = activityRegistryPopulator;
    public async Task StartAsync(CancellationToken cancellationToken) => await _activityRegistryPopulator.PopulateRegistryAsync(cancellationToken);
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}