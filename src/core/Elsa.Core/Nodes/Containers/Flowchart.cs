using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Nodes.Containers
{
    public record Connection(INode Source, INode Target);
    
    public class Flowchart : Container
    {
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }
}