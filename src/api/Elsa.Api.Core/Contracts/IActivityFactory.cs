using System;
using Elsa.Api.Core.Models;
using Elsa.Contracts;

namespace Elsa.Api.Core.Contracts;

public interface IActivityFactory
{
    IActivity Create(Type type, ActivityConstructorContext context);
}