using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IIdentityGraphService
    {
        IEnumerable<NodeIdentity> CreateIdentityGraph(Node root);
        IEnumerable<NodeIdentity> AssignIdentities(Node root);
    }
}