using System;
using System.Text.Json;
using Elsa.Api.Core.Contracts;
using Elsa.Api.Core.Models;
using Elsa.Contracts;

namespace Elsa.Api.Core.Services;

public class ActivityFactory : IActivityFactory
{
    public IActivity Create(Type type, ActivityConstructorContext context)
    {
        var activity = (IActivity)context.Element.Deserialize(type, context.SerializerOptions)!;
        return activity;
    }
}