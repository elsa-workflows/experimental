using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Runtime.Contracts;

namespace Elsa.Activities.Http
{
    public class HttpEndpoint : Activity, ITrigger
    {
        [Input] public Input<string> Path { get; set; } = default!;
        [Input] public Input<ICollection<string>> SupportedMethods { get; set; } = new(new[] { HttpMethod.Get.Method });
        [Output] public Output<HttpRequestModel>? Result { get; set; }

        protected override void Execute(ActivityExecutionContext context)
        {
            // If the activity triggered the workflow, do nothing.
            if (context.GetActivityIsCurrentTrigger(this))
                return;

            // Create bookmarks.
            var bookmarks = CreateBookmarks(context).ToList();
            context.SetBookmarks(bookmarks);
        }

        private IEnumerable<Bookmark> CreateBookmarks(ActivityExecutionContext context)
        {
            var path = context.Get<string>(Path.LocationReference)!;
            var methods = context.Get<ICollection<string>>(SupportedMethods.LocationReference)!;
            var hasher = context.GetRequiredService<IHasher>();

            foreach (var method in methods)
            {
                var hashInput = (path.ToLowerInvariant(), method.ToLowerInvariant());
                var hash = hasher.Hash(hashInput);
                yield return new Bookmark(Guid.NewGuid().ToString(), ActivityType, hash, ActivityId);
            }
        }
    }
}