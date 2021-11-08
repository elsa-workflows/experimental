using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Contracts;

namespace Elsa.Api.Core.Services
{
    /// <summary>
    /// Populates the <see cref="IActivityDescriptorRegistry"/> with descriptors provided by <see cref="IActivityDescriptorProvider"/>s. 
    /// </summary>
    public class ActivityDescriptorRegistryPopulator : IActivityDescriptorRegistryPopulator
    {
        private readonly IEnumerable<IActivityDescriptorProvider> _providers;
        private readonly IActivityDescriptorRegistry _registry;

        public ActivityDescriptorRegistryPopulator(IEnumerable<IActivityDescriptorProvider> providers, IActivityDescriptorRegistry registry)
        {
            _providers = providers;
            _registry = registry;
        }

        public async ValueTask PopulateRegistryAsync(CancellationToken cancellationToken)
        {
            var tasks = _providers.Select(async x => await x.GetDescriptorsAsync(cancellationToken));
            var descriptorLists = await Task.WhenAll(tasks);
            var descriptors = descriptorLists.SelectMany(x => x);
            _registry.AddDescriptors(descriptors);
        }
    }
}