using System;

namespace Elsa.Contracts
{
    public interface IExpressionHandlerRegistry
    {
        void Register(Type expression, Type handler);
        IExpressionHandler? GetHandler<T>(IExpression<T> expression);
    }
}