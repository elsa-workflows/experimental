using System;
using System.Text.Json;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Api.Core.Models;

public class ExpressionSyntaxDescriptor
{
    public string Syntax { get; init; } = default!;
    public Func<ExpressionConstructorContext, IExpression> CreateExpression { get; init; } = default!;
    public Func<LocationReferenceConstructorContext, RegisterLocationReference> CreateLocationReference { get; init; } = default!;
}

public record ExpressionConstructorContext(JsonElement Element, JsonSerializerOptions SerializerOptions);

public record LocationReferenceConstructorContext(IExpression Expression)
{
    public T GetExpression<T>() => (T)Expression;
}