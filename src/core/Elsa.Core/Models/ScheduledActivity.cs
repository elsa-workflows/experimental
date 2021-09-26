using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class ScheduledActivity
    {
        public ScheduledActivity(IActivity activity, IActivity? owner = default, IEnumerable<RegisterLocationReference>? locationReferences = default)
        {
            Activity = activity;
            Owner = owner;
            LocationReferences = locationReferences?.ToList();
        }

        public IActivity Activity { get; }
        public IActivity? Owner { get; }
        public ICollection<RegisterLocationReference>? LocationReferences { get; set; }
    }

    public interface IWork
    {
        ValueTask ExecuteAsync
    }
}