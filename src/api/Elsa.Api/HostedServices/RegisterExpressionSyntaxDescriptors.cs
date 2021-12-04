using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Contracts;
using Microsoft.Extensions.Hosting;

namespace Elsa.Api.HostedServices;

public class RegisterExpressionSyntaxDescriptors : IHostedService
{
    private readonly IExpressionSyntaxRegistryPopulator _expressionSyntaxRegistryPopulator;
    public RegisterExpressionSyntaxDescriptors(IExpressionSyntaxRegistryPopulator expressionSyntaxRegistryPopulator) => _expressionSyntaxRegistryPopulator = expressionSyntaxRegistryPopulator;
    public async Task StartAsync(CancellationToken cancellationToken) => await _expressionSyntaxRegistryPopulator.PopulateRegistryAsync(cancellationToken);
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}