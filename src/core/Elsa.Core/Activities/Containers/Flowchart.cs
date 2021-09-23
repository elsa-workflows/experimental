using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.Containers
{
    public record Connection(IActivity Source, IActivity Target);
    
    public class Flowchart : Container
    {
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
        
        protected override void ScheduleChildren(ActivityExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}