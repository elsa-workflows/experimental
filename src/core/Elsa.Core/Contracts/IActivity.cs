using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivity : INode
    {
        string ActivityType { get; set; }
        IDictionary<string, object> Metadata { get; set; }
        ValueTask ExecuteAsync(ActivityExecutionContext context);
    }
}