using System;
using System.Collections.Generic;

namespace Elsa.Api.Core.Options;

public class ApiOptions
{
    /// <summary>
    /// A collection of activity types made available from the API.
    /// </summary>
    public HashSet<Type> ActivityTypes { get; set; } = new();
}