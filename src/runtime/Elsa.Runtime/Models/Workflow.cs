using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Runtime.Models
{
    public class Workflow
    {
        public Workflow(string id, IActivity root, IEnumerable<Bookmark> triggers)
        {
            Id = id;
            Root = root;
            Triggers = triggers.ToList();
        }
        
        public string Id { get; }
        public IActivity Root { get; }
        public ICollection<Bookmark> Triggers { get; }
    }
}