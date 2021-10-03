using System.Collections.Generic;
using System.Linq;
using Elsa.Models;
using Elsa.Persistence.Abstractions.Models;

namespace Elsa.Runtime.Extensions
{
    public static class WorkflowCollectionExtensions
    {
        public static PagedList<WorkflowDefinition> Paginate(this IEnumerable<WorkflowDefinition> workflows, PagerParameters pagerParameters)
        {
            var query = from workflowDefinition in workflows select workflowDefinition;
            var limit = pagerParameters.Limit;
            var take = limit + 1;
            var cursor = pagerParameters.Cursor;

            if (pagerParameters.Direction == CursorDirection.Next)
                query = query.OrderBy(x => x.CreatedAt);
            else
                query = query.OrderByDescending(x => x.CreatedAt);

            if (cursor != null)
            {
                if (pagerParameters.Direction == CursorDirection.Next)
                    query = query.Where(x => x.Id == cursor.Id && x.CreatedAt > cursor.Sort);
                else
                    query = query.Where(x => x.Id == cursor.Id && x.CreatedAt <= cursor.Sort);
            }

            var results = query.Take(take).ToList();
            var selection = results.Take(limit).ToList();
            var next = results.Skip(limit).FirstOrDefault();
            var nextCursor = next != null ? new Cursor(next.Id, next.CreatedAt) : default;
            return new PagedList<WorkflowDefinition>(selection, limit, nextCursor);
        }
    }
}