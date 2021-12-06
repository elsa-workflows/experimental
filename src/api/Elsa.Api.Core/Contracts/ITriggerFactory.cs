using System;
using Elsa.Api.Core.Models;
using Elsa.Contracts;

namespace Elsa.Api.Core.Contracts;

public interface ITriggerFactory
{
    ITrigger Create(Type type, TriggerConstructorContext context);
}