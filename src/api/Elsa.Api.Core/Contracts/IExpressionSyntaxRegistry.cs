using System;
using System.Collections.Generic;
using Elsa.Api.Core.Models;

namespace Elsa.Api.Core.Contracts;

public interface IExpressionSyntaxRegistry
{
    void Add(ExpressionSyntaxDescriptor descriptor);
    void AddMany(IEnumerable<ExpressionSyntaxDescriptor> descriptors);
    IEnumerable<ExpressionSyntaxDescriptor> ListAll();
    ExpressionSyntaxDescriptor? Find(Func<ExpressionSyntaxDescriptor, bool> predicate);
    ExpressionSyntaxDescriptor? Find(string syntax);
}