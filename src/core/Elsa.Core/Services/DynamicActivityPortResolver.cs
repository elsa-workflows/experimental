using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class DynamicActivityPortResolver : IActivityPortResolver
    {
        public bool GetSupportsActivity(IActivity activity) => activity is DynamicActivity; 

        public IEnumerable<IActivity> GetPorts(IActivity activity)
        {
            var ports = ((DynamicActivity)activity).Ports.Values;
            return ports.Where(x => x != null).Select(x => x!);
        }
    }
}