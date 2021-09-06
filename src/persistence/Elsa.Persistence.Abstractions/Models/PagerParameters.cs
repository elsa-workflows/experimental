namespace Elsa.Persistence.Abstractions.Models
{
    public record PagerParameters(int Limit, CursorDirection Direction = CursorDirection.Next, Cursor? Cursor = default);
}