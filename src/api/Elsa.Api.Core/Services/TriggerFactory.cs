using System;
using System.Text.Json;
using Elsa.Api.Core.Contracts;
using Elsa.Api.Core.Models;
using Elsa.Contracts;

namespace Elsa.Api.Core.Services;

public class TriggerFactory : ITriggerFactory
{
    public ITrigger Create(Type type, TriggerConstructorContext context)
    {
        var trigger = (ITrigger)context.Element.Deserialize(type, context.SerializerOptions)!;
        return trigger;
    }
}