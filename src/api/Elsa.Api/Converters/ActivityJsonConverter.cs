using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Elsa.Activities.Console;
using Elsa.Contracts;

namespace Elsa.Api.Converters
{
    /// <summary>
    /// Serializes <see cref="IActivity"/> objects.
    /// </summary>
    public class ActivityJsonConverter : JsonConverter<IActivity>
    {
        public override bool CanConvert(Type type) => type.IsAssignableFrom(typeof(IActivity));

        public override IActivity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!JsonDocument.TryParseValue(ref reader, out var doc))
                throw new JsonException("Failed to parse JsonDocument");

            if (!doc.RootElement.TryGetProperty("activityType", out var activityTypeNameElement))
                throw new JsonException("Failed to extract activity type property");

            var activityTypeName = activityTypeNameElement.GetString();
            var rootElement = doc.RootElement.GetRawText();
            var activityType = typeof(WriteLine);

            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.Add(new InputJsonConverterFactory());
            newOptions.Converters.Remove(newOptions.Converters.First(x => x is ActivityJsonConverter));
            
            var activity = JsonSerializer.Deserialize(rootElement, activityType, newOptions)!;

            return (IActivity)activity;
        }

        public override void Write(Utf8JsonWriter writer, IActivity value, JsonSerializerOptions options)
        {
            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.Remove(newOptions.Converters.First(x => x is ActivityJsonConverter));
            newOptions.Converters.Add(new InputJsonConverterFactory());
            JsonSerializer.Serialize(writer, value, value.GetType(), newOptions);
        }
    }
}