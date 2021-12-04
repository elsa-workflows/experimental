using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Contracts;

namespace Elsa.Api.Core.Services;

/// <summary>
/// Populates the <see cref="IActivityRegistry"/> with descriptors provided by <see cref="IActivityProvider"/>s. 
/// </summary>
public class ActivityRegistryPopulator : IActivityRegistryPopulator
{
    private readonly IEnumerable<IActivityProvider> _providers;
    private readonly IActivityRegistry _registry;

    public ActivityRegistryPopulator(IEnumerable<IActivityProvider> providers, IActivityRegistry registry)
    {
        _providers = providers;
        _registry = registry;
    }

    public async ValueTask PopulateRegistryAsync(CancellationToken cancellationToken)
    {
        foreach (var provider in _providers)
        {
            var descriptors = await provider.GetDescriptorsAsync(cancellationToken);
            _registry.AddMany(provider, descriptors);
        }
    }
}