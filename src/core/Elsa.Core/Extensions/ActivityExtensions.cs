using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Extensions
{
    public static class ActivityExtensions
    {
        public static IEnumerable<Input> GetInputs(this IActivity activity)
        {
            var inputProps = activity.GetType().GetProperties().Where(x => typeof(Input).IsAssignableFrom(x.PropertyType)).ToList();

            var query =
                from inputProp in inputProps
                select (Input?)inputProp.GetValue(activity)
                into input
                where input != null
                select input;

            return query.Select(x => x!).ToList();
        }
    }
}