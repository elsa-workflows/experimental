using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public abstract class NodeDriver<TNode> : INodeDriver
    {
        public virtual bool GetSupportsNode(INode node) => node is TNode;
        public virtual int Priority => 0;
        ValueTask<INodeExecutionResult> INodeDriver.ExecuteAsync(NodeExecutionContext context) => ExecuteAsync((TNode)context.Node, context);
        protected abstract ValueTask<INodeExecutionResult> ExecuteAsync(TNode node, NodeExecutionContext context);
    }
}