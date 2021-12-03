using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Elsa.Models;

namespace Elsa.Api.Converters;

public class InputJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeof(Input).IsAssignableFrom(typeToConvert);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var type = typeToConvert.GetGenericArguments().First();
        return (JsonConverter)Activator.CreateInstance(typeof(InputJsonConverter<>).MakeGenericType(type))!;
    }
}