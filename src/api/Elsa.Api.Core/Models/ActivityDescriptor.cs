using System;
using System.Collections.Generic;
using System.Text.Json;
using Elsa.Contracts;

namespace Elsa.Api.Core.Models;

public class ActivityDescriptor
{
    public string ActivityType { get; init; } = default!;
    public string Category { get; init; } = default!;
    public Func<ActivityConstructorContext, IActivity> Constructor { get; init; } = default!;
    public string? DisplayName { get; init; }
    public string? Description { get; init; }
    public ICollection<Port> InboundPorts { get; init; } = new List<Port>();
    public ICollection<Port> OutboundPorts { get; init; } = new List<Port>();
    public ICollection<ActivityInputDescriptor> InputProperties { get; init; } = new List<ActivityInputDescriptor>();
    public ICollection<ActivityOutputDescriptor> OutputProperties { get; init; } = new List<ActivityOutputDescriptor>();
}

public record ActivityConstructorContext(JsonElement Element, JsonSerializerOptions SerializerOptions);