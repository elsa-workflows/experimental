using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Pipelines.NodeExecution.Components
{
    public class ParentDriversMiddleware
    {
        private readonly NodeMiddlewareDelegate _next;
        private readonly INodeDriverRegistry _driverRegistry;

        public ParentDriversMiddleware(NodeMiddlewareDelegate next, INodeDriverRegistry driverRegistry)
        {
            _next = next;
            _driverRegistry = driverRegistry;
        }

        public async ValueTask InvokeAsync(NodeExecutionContext context)
        {
            // Invoke next middleware.
            await _next(context);

            // If no bookmarks were created.
            if (context.Bookmarks.Any())
                return;
            
            // Notify node's parent (if any) that its child has executed.    
            var node = context.Node;
            var owner = context.WorkflowExecutionContext.Graph.First(x => x.Node == node).Parent?.Node;

            if (owner != null)
            {
                var ownerDriver = _driverRegistry.GetDriver(owner);

                if (ownerDriver is INotifyNodeExecuted parentDriver)
                    await parentDriver.OnChildComplete(context, owner);
            }
        }
    }

    public static class NotifyParentMiddlewareExtensions
    {
        public static INodeExecutionBuilder UseParentDrivers(this INodeExecutionBuilder builder) => builder.UseMiddleware<ParentDriversMiddleware>();
    }
}