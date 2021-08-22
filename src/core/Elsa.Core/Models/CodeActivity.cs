using Elsa.Contracts;

namespace Elsa.Models
{
    public abstract class CodeActivity : IActivity
    {
        public string ActivityId { get; set; } = default!;
        public string ActivityType => GetType().Name;
    }
}