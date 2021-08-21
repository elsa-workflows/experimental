namespace Elsa.Contracts
{
    public interface INode
    {
        string? Name { get; set; }
        NodeStatus Status { get; set; }
    }
}