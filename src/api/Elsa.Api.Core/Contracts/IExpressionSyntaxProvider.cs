using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Models;

namespace Elsa.Api.Core.Contracts;

public interface IExpressionSyntaxProvider
{
    ValueTask<IEnumerable<ExpressionSyntaxDescriptor>> GetDescriptorsAsync(CancellationToken cancellationToken = default);
}