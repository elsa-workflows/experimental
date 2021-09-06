using System;

namespace Elsa.Persistence.Abstractions.Models
{
    public record Cursor(string Id, DateTime Sort);
}