using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.Extensions
{
    public static class GraphNodeExtensions
    {
        public static IEnumerable<Node> Flatten(this Node root)
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