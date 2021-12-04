using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Elsa.Api.Core.Contracts;
using Elsa.Api.Core.Models;
using Elsa.Contracts;

namespace Elsa.Api.Converters
{
    /// <summary>
    /// Serializes <see cref="IActivity"/> objects.
    /// </summary>
    public class ActivityJsonConverter : JsonConverter<IActivity>
    {
        private readonly IActivityRegistry _activityRegistry;
        private readonly IServiceProvider _serviceProvider;

        public ActivityJsonConverter(IActivityRegistry activityRegistry, IServiceProvider serviceProvider)
        {
            _activityRegistry = activityRegistry;
            _serviceProvider = serviceProvider;
        }
        
        public override bool CanConvert(Type type) => type.IsAssignableFrom(typeof(IActivity));

        public override IActivity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!JsonDocument.TryParseValue(ref reader, out var doc))
                throw new JsonException("Failed to parse JsonDocument");

            if (!doc.RootElement.TryGetProperty("activityType", out var activityTypeNameElement))
                throw new JsonException("Failed to extract activity type property");

            var activityTypeName = activityTypeNameElement.GetString()!;
            var activityDescriptor = _activityRegistry.Find(activityTypeName);

            if (activityDescriptor == null)
                throw new Exception($"Activity of type {activityTypeName} not found in registry");

            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.Add(new InputJsonConverterFactory(_serviceProvider));
            newOptions.Converters.Remove(newOptions.Converters.First(x => x is ActivityJsonConverter));
            
            var context = new ActivityConstructorContext(doc.RootElement, newOptions);
            var activity = activityDescriptor.Constructor(context);

            return activity;
        }

        public override void Write(Utf8JsonWriter writer, IActivity value, JsonSerializerOptions options)
        {
            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.Remove(newOptions.Converters.First(x => x is ActivityJsonConverter));
            newOptions.Converters.Add(new InputJsonConverterFactory(_serviceProvider));
            JsonSerializer.Serialize(writer, value, value.GetType(), newOptions);
        }
    }
}