using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Api.Core.Contracts;

public interface ITriggerRegistryPopulator
{
    ValueTask PopulateRegistryAsync(CancellationToken cancellationToken);
}