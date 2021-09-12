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
        private readonly IHasher _hasher;

        public HttpEndpointDriver(IHasher hasher)
        {
            _hasher = hasher;
        }

        protected override void Execute(HttpEndpoint activity, ActivityExecutionContext context)
        {
            // If the activity triggered the workflow, do nothing.
            if (context.GetActivityIsCurrentTrigger(activity))
                return;

            // Create bookmarks.
            var bookmarks = CreateBookmarks(activity, context).ToList();
            context.SetBookmarks(bookmarks);
        }

        private IEnumerable<Bookmark> CreateBookmarks(HttpEndpoint activity, ActivityExecutionContext context)
        {
            var path = context.Get<string>(activity.Path.LocationReference)!;
            var methods = context.Get<ICollection<string>>(activity.SupportedMethods.LocationReference)!;

            foreach (var method in methods)
            {
                var hashInput = (path.ToLowerInvariant(), method.ToLowerInvariant());
                var hash = _hasher.Hash(hashInput);
                yield return new Bookmark(Guid.NewGuid().ToString(), activity.ActivityType, hash, activity.ActivityId);
            }
        }
    }
}