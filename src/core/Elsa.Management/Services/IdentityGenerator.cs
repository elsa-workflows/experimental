using Elsa.Management.Contracts;

namespace Elsa.Management.Services;

public class IdentityGenerator : IIdentityGenerator
{
    public string GenerateId() => Guid.NewGuid().ToString("N");
}