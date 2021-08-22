namespace Elsa.Contracts
{
    public interface IActivity
    {
        string ActivityId { get; set; }
        string ActivityType { get; }
    }
}