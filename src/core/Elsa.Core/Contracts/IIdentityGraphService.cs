using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IIdentityGraphService
    {
        void AssignIdentities(IActivity root);
        void AssignIdentities(ActivityNode root);
    }
}