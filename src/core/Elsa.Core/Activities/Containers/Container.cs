using System.Collections.Generic;
using System.Collections.ObjectModel;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.Containers
{
    public abstract class Container : Activity
    {
        public Container()
        {
        }

        public Container(params IActivity[] activities)
        {
            Activities = activities;
        }
        
        [Ports]public ICollection<IActivity> Activities { get; set; } = new List<IActivity>();
        public ICollection<Variable> Variables { get; set; } = new Collection<Variable>();
    }
}