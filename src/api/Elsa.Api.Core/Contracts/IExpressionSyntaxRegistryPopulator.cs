using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Api.Core.Contracts;

public interface IExpressionSyntaxRegistryPopulator
{
    ValueTask PopulateRegistryAsync(CancellationToken cancellationToken);
}