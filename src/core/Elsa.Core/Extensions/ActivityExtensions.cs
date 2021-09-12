using System.Collections.Generic;
using System.Linq;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Extensions
{
    public static class ActivityExtensions
    {
        public static IEnumerable<IExpression> GetInputExpressions(this IActivity activity)
        {
            var inputProps = activity.GetType().GetProperties().Where(x => typeof(Input).IsAssignableFrom(x.PropertyType)).ToList();

            return
                from inputProp in inputProps
                select (Input?)inputProp.GetValue(activity)
                into input
                where input != null
                select input.Expression;
        }
    }
}