using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Results
{
    public class BookmarkResult : INodeExecutionResult
    {
        public BookmarkResult(string name, IDictionary<string, object?>? data = default, Action? resume = default)
        {
            Name = name;
            Resume = resume;
            Data = data ?? new Dictionary<string, object?>();
        }

        public string Name { get; set; }
        public Action? Resume { get; }
        public IDictionary<string, object?> Data { get; set; }
        
        public ValueTask ExecuteAsync(NodeExecutionContext context)
        {
            var bookmark = new Bookmark(context.Node, Name, Data, Resume);
            context.SetBookmark(bookmark);
            return new ValueTask();
        }
    }
}