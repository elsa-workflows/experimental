using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Pipelines.ActivityExecution;

public delegate ValueTask ActivityMiddlewareDelegate(ActivityExecutionContext context);