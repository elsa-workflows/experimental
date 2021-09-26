using System;
using System.Threading.Tasks;

namespace Elsa.Models
{
    public class ActivityWorkItem
    {
        public ActivityWorkItem(string activityId, Func<ValueTask> execute)
        {
            ActivityId = activityId;
            Execute = execute;
        }

        public string ActivityId { get; }
        public Func<ValueTask> Execute { get; }
    }
}