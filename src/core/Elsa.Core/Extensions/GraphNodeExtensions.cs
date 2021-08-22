using System.Collections.Generic;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Extensions
{
    public static class GraphNodeExtensions
    {
        public static IEnumerable<GraphNode> Flatten(this GraphNode root)
        {
            yield return root;

            foreach (var node in root.Children)
            {
                var children = node.Flatten();

                foreach (var child in children)
                    yield return child;
            }
        }
    }
}