using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Services
{
    public class CodeActivityPortResolver : IActivityPortResolver
    {
        public bool GetSupportsActivity(IActivity activity) => activity is CodeActivity;

        public IEnumerable<IActivity> GetPorts(IActivity activity) =>
            GetSinglePorts(activity)
                .Concat(GetManyPorts(activity))
                .Where(x => x != null)
                .Select(x => x!)
                .ToHashSet();

        private static IEnumerable<IActivity?> GetSinglePorts(IActivity activity)
        {
            var nodeType = activity.GetType();

            var ports =
                from prop in nodeType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where typeof(IActivity).IsAssignableFrom(prop.PropertyType)
                let portAttr = prop.GetCustomAttribute<PortAttribute>()
                where portAttr != null
                select (IActivity)prop.GetValue(activity)!;

            return ports;
        }

        private static IEnumerable<IActivity?> GetManyPorts(IActivity activity)
        {
            var nodeType = activity.GetType();

            var ports =
                from prop in nodeType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where typeof(IEnumerable<IActivity>).IsAssignableFrom(prop.PropertyType)
                let portsAttr = prop.GetCustomAttribute<PortsAttribute>()
                where portsAttr != null
                select (IEnumerable<IActivity>)prop.GetValue(activity)!;

            return ports.SelectMany(x => x);
        }
    }
}