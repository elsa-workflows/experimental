using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Elsa.Contracts;

namespace Elsa.Options
{
    public class WorkflowEngineOptions
    {
        private readonly IDictionary<Type, Type> _nodeDrivers = new Dictionary<Type, Type>();
        private readonly IDictionary<Type, Type> _expressionHandlers = new Dictionary<Type, Type>();

        public WorkflowEngineOptions()
        {
            NodeDrivers = new ReadOnlyDictionary<Type, Type>(_nodeDrivers);
            ExpressionHandlers = new ReadOnlyDictionary<Type, Type>(_expressionHandlers);
        }
        
        public IDictionary<Type, Type> NodeDrivers { get; }
        public IDictionary<Type, Type> ExpressionHandlers { get; }
        public WorkflowEngineOptions RegisterNodeDriver<TNode, TDriver>() where TNode : INode where TDriver : INodeDriver => RegisterNodeDriver(typeof(TNode), typeof(TDriver));

        public WorkflowEngineOptions RegisterNodeDriver(Type node, Type driver)
        {
            _nodeDrivers.Add(node, driver);
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