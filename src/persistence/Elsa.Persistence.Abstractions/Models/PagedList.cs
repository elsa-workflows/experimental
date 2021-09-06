using System.Collections.Generic;

namespace Elsa.Persistence.Abstractions.Models
{
    public record PagedList<T>(ICollection<T> Items, int PageSize, Cursor? Cursor);
}