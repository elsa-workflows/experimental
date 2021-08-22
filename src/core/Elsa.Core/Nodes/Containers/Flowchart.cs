using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Nodes.Containers
{
    public record Connection(IActivity Source, IActivity Target);
    
    public class Flowchart : Container
    {
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }
}