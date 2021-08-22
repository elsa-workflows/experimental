using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Elsa.Contracts;

namespace Elsa.Options
{
    public class WorkflowEngineOptions
    {
        private readonly IDictionary<string, Type> _nodeDrivers = new Dictionary<string, Type>();
        private readonly IDictionary<Type, Type> _expressionHandlers = new Dictionary<Type, Type>();

        public WorkflowEngineOptions()
        {
            Drivers = new ReadOnlyDictionary<string, Type>(_nodeDrivers);
            ExpressionHandlers = new ReadOnlyDictionary<Type, Type>(_expressionHandlers);
        }
        
        public IDictionary<string, Type> Drivers { get; }
        public IDictionary<Type, Type> ExpressionHandlers { get; }
        public WorkflowEngineOptions RegisterNodeDriver<TNode, TDriver>() where TNode : IActivity where TDriver : IActivityDriver => RegisterNodeDriver(typeof(TNode).Name, typeof(TDriver));

        public WorkflowEngineOptions RegisterNodeDriver(string nodeType, Type driver)
        {
            _nodeDrivers.Add(nodeType, driver);
            return this;
        }

        public WorkflowEngineOptions RegisterExpressionHandler(Type expression, Type handler)
        {
            _expressionHandlers.Add(expression, handler);
            return this;
        }

        public WorkflowEngineOptions RegisterExpressionHandler<THandler>(Type expression)
        {
            _expressionHandlers.Add(expression, typeof(THandler));
            return this;
        }
    }
}