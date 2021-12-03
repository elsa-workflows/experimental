using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Elsa.Expressions;
using Elsa.Models;

namespace Elsa.Api.Converters
{
    /// <summary>
    /// Serializes <see cref="Input"/> objects.
    /// </summary>
    public class InputJsonConverter<T> : JsonConverter<Input<T>>
    {
        public override bool CanConvert(Type typeToConvert) => typeof(Input).IsAssignableFrom(typeToConvert);

        public override Input<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!JsonDocument.TryParseValue(ref reader, out var doc))
                throw new JsonException("Failed to parse JsonDocument");

            if (!doc.RootElement.TryGetProperty("targetType", out var inputTargetTypeElement))
                throw new JsonException("Failed to extract input type property");
            
            var expressionElement = doc.RootElement.GetProperty("expression");

            if (!expressionElement.TryGetProperty("expressionType", out var expressionTypeNameElement))
                throw new JsonException("Failed to extract expression type property");

            var expressionTypeName = expressionTypeNameElement.GetString();

            switch (expressionTypeName)
            {
                case "Literal":
                    var expressionValue = expressionElement.GetProperty("value").GetString();
                    var expression = new Literal(expressionValue);
                    return new Input<T>(expression);
            }
            
            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, Input<T> value, JsonSerializerOptions options)
        {
            var expression = value.Expression;
            var targetType = value.TargetType;

            var model = new
            {
                TargetType = targetType,
                Expression = new
                {
                    ExpressionType = "Literal",
                    Value = (expression as LiteralExpression)!.Value
                }
            };

            JsonSerializer.Serialize(writer, model, options);
        }
    }
}