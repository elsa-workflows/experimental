using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivityDriver
    {
        ValueTask ExecuteAsync(ActivityExecutionContext context);
    }
}