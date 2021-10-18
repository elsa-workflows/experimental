using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Runtime.Contracts
{
    public interface IStimulusDispatcher
    {
        Task DispatchStimulus(IStimulus stimulus, CancellationToken cancellationToken = default);
    }
}