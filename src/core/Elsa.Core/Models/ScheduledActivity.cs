using Elsa.Contracts;

namespace Elsa.Models
{
    public record ScheduledActivity(IActivity Activity, IActivity? Owner = default);
}