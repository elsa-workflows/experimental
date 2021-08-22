using System.Collections.Generic;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.Containers
{
    public abstract class Container : CodeActivity
    {
        public Container()
        {
        }

        public Container(params IActivity[] activities)
        {
            Activities = activities;
        }
        
        [Ports]public ICollection<IActivity> Activities { get; set; } = new List<IActivity>();
        public IDictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();
    }
}