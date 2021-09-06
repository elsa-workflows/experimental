using System.Collections.Generic;
using Elsa.Models;
using Elsa.Runtime.Contracts;

namespace Elsa.Runtime.Stimuli
{
    public record StandardStimulus(string ActivityTypeName, string? Hash = default, IDictionary<string, object?>? Data = default) : IStimulus;
}