using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Elsa.Api.Converters;
using Elsa.Api.Core.Contracts;
using Microsoft.AspNetCore.Http;

namespace Elsa.Api.Endpoints.ActivityDescriptors
{
    public static class List
    {
        public static IResult HandleAsync(IActivityDescriptorRegistry registry, IWellKnownTypeRegistry wellKnownTypeRegistry)
        {
            var descriptors = registry.ListAll().ToList();

            var model = new
            {
                ActivityDescriptors = descriptors
            };

            return Results.Json(model, new JsonSerializerOptions
            {
                Converters = { new TypeJsonConverter(wellKnownTypeRegistry) },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            });
        }
    }
}