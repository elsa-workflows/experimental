using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Api.Core.Contracts
{
    public interface IActivityDescriptorRegistryPopulator
    {
        ValueTask PopulateRegistryAsync(CancellationToken cancellationToken);
    }
}