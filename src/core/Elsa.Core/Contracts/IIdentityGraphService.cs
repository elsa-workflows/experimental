using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IIdentityGraphService
    {
        IEnumerable<NodeIdentity> CreateIdentityGraph(Node root);
        void AssignIdentities(Node root);
        void AssignIdentities(IActivity root);
    }
}