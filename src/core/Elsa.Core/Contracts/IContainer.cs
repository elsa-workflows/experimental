using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.Contracts;

public interface IContainer : IActivity
{
    ICollection<Variable> Variables { get; }
}