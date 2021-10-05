using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Antlr4.Runtime.Tree;
using Elsa.Activities.Containers;
using Elsa.Builders;
using Elsa.Contracts;
using Elsa.Dsl.Extensions;
using Elsa.Models;

namespace Elsa.Dsl.Interpreters
{
    public class WorkflowDefinitionBuilderInterpreter : ElsaParserBaseVisitor<IWorkflowDefinitionBuilder>
    {
        private readonly ITypeSystem _typeSystem;
        private readonly IWorkflowDefinitionBuilder _workflowDefinitionBuilder = new WorkflowDefinitionBuilder();
        private readonly ParseTreeProperty<object> _object = new();
        private readonly Stack<PropertyInfo> _propertyStack = new();
        private readonly Stack<object> _propertyValueStack = new();

        public WorkflowDefinitionBuilderInterpreter(ITypeSystem typeSystem)
        {
            _typeSystem = typeSystem;
        }

        protected override IWorkflowDefinitionBuilder DefaultResult => _workflowDefinitionBuilder;

        public override IWorkflowDefinitionBuilder VisitFile(ElsaParser.FileContext context)
        {
            var triggerStats = context.stat().OfType<ElsaParser.TriggerStatContext>().ToList();
            var objectStats = context.stat().OfType<ElsaParser.ObjectStatContext>().ToList();

            VisitMany(triggerStats);
            VisitMany(objectStats);

            var rootActivities = objectStats.Select(x => (IActivity)_object.Get(x.@object())).ToList();
            var rootActivity = rootActivities.Count == 1 ? rootActivities.Single() : new Sequence(rootActivities.ToArray());
            _workflowDefinitionBuilder.WithRoot(rootActivity);

            return DefaultResult;
        }
        
        public override IWorkflowDefinitionBuilder VisitTrigger(ElsaParser.TriggerContext context)
        {
            VisitChildren(context);
            var trigger = (ITrigger)_object.Get(context.@object());

            _workflowDefinitionBuilder.AddTrigger(trigger);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitObject(ElsaParser.ObjectContext context)
        {
            var objectTypeName = context.ID().GetText();
            var objectTypeDescriptor = _typeSystem.ResolveTypeName(objectTypeName);

            if (objectTypeDescriptor == null)
                throw new Exception($"Unknown type: {objectTypeName}");

            var objectType = objectTypeDescriptor.Type;
            var @object = Activator.CreateInstance(objectType)!;

            _object.Put(context, @object);
            VisitChildren(context);
            _propertyValueStack.Push(@object);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitProperty(ElsaParser.PropertyContext context)
        {
            var @object = _object.Get(context.Parent.Parent.Parent);
            var objectType = @object.GetType();
            var propertyName = context.ID().GetText();
            var propertyInfo = objectType.GetProperty(propertyName);

            if (propertyInfo == null)
                throw new Exception($"Type {objectType.Name} does not have a public property named {propertyName}.");

            _propertyStack.Push(propertyInfo);
            VisitChildren(context);
            _propertyStack.Pop();

            var propertyValue = _propertyValueStack.Pop();
            SetPropertyValue(@object, propertyInfo, propertyValue);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitBracketsExpr(ElsaParser.BracketsExprContext context)
        {
            var propertyInfo = _propertyStack.Peek();
            var propertyType = GetUnderlyingTargetType(propertyInfo.PropertyType);
            var targetElementType = propertyType.GetGenericArguments().First();
            var contents = context.exprList().expr();

            var items = contents.Select(x =>
            {
                Visit(x);
                return _propertyValueStack.Pop();
            }).ToList();

            var stronglyTypedListType = typeof(ICollection<>).MakeGenericType(targetElementType);
            var stronglyTypedList = items.ConvertTo(stronglyTypedListType);

            _propertyValueStack.Push(stronglyTypedList);
            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitStringValueExpr(ElsaParser.StringValueExprContext context)
        {
            var value = context.GetText().Trim('\"');
            _propertyValueStack.Push(value);
            return DefaultResult;
        }

        private void SetPropertyValue(object target, PropertyInfo propertyInfo, object? value)
        {
            if (typeof(Input).IsAssignableFrom(propertyInfo.PropertyType))
                value = CreateInputValue(propertyInfo, value);

            propertyInfo.SetValue(target, value, null);
        }

        private Input CreateInputValue(PropertyInfo propertyInfo, object? propertyValue)
        {
            var underlyingType = propertyInfo.PropertyType.GetGenericArguments().First();
            var inputType = typeof(Input<>).MakeGenericType(underlyingType);
            var convertedValue = propertyValue.ConvertTo(underlyingType);
            var inputValue = (Input)Activator.CreateInstance(inputType, convertedValue)!;
            return inputValue;
        }

        private void VisitMany(IEnumerable<IParseTree> contexts)
        {
            foreach (var parseTree in contexts) Visit(parseTree);
        }

        private Type GetUnderlyingTargetType(Type type) => typeof(Input).IsAssignableFrom(type) ? type.GetGenericArguments().First() : type;
    }
}