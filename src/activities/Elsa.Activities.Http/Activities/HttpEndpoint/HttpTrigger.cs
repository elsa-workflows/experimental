using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.Http
{
    public class HttpTrigger : Trigger
    {
        [Input] public Input<string> Path { get; set; } = default!;
        [Input] public Input<ICollection<string>> SupportedMethods { get; set; } = new(new[] { HttpMethod.Get.Method });
        [Output] public Output<HttpRequestModel>? Result { get; set; }
        
        protected override IEnumerable<object> GetHashInputs(TriggerIndexingContext context)
        {
            var path = context.ExpressionExecutionContext.Get(Path);
            var methods = context.ExpressionExecutionContext.Get(SupportedMethods);
            return methods!.Select(x => (path!.ToLowerInvariant(), x.ToLowerInvariant())).Cast<object>().ToArray();
        }

        protected override void Execute(ActivityExecutionContext context)
        {
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
                yield return new Bookmark(Guid.NewGuid().ToString(), ActivityType, hash, Id, context.Id);
            }
        }
    }
}