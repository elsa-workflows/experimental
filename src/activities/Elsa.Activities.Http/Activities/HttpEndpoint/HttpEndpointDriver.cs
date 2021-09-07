using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.Http
{
    public class HttpEndpointDriver : ActivityDriver<HttpEndpoint>
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IHasher _hasher;

        public HttpEndpointDriver(IExpressionEvaluator expressionEvaluator, IHasher hasher)
        {
            _expressionEvaluator = expressionEvaluator;
            _hasher = hasher;
        }

        protected override async ValueTask ExecuteAsync(HttpEndpoint activity, ActivityExecutionContext context)
        {
            // If the activity triggered the workflow, do nothing.
            if (context.GetActivityIsCurrentTrigger(activity))
                return;

            // Create bookmarks.
            var bookmarks = await CreateBookmarksAsync(activity, context).ToListAsync(context.CancellationToken);
            context.SetBookmarks(bookmarks);
        }

        private async IAsyncEnumerable<Bookmark> CreateBookmarksAsync(HttpEndpoint activity, ActivityExecutionContext context)
        {
            var path = await _expressionEvaluator.EvaluateAsync(activity.Path, new ExpressionExecutionContext());
            var methods = await _expressionEvaluator.EvaluateAsync(activity.SupportedMethods, new ExpressionExecutionContext());

            foreach (var method in methods)
            {
                var hashInput = (path.ToLowerInvariant(), method.ToLowerInvariant());
                var hash = _hasher.Hash(hashInput);
                yield return new Bookmark(Guid.NewGuid().ToString(), activity.ActivityType, hash, activity.ActivityId);
            }
        }
    }
}