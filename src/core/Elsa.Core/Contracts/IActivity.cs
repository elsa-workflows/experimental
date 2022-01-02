using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts;

public interface IActivity : INode
{
    ValueTask ExecuteAsync(ActivityExecutionContext context);
}