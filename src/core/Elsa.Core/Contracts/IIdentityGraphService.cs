using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IIdentityGraphService
    {
        IEnumerable<NodeIdentity> CreateIdentityGraph(INode root);
    }
}