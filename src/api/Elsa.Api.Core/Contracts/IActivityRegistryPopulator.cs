using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Api.Core.Contracts;

public interface IActivityRegistryPopulator
{
    ValueTask PopulateRegistryAsync(CancellationToken cancellationToken);
}