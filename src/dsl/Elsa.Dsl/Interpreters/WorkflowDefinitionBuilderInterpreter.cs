using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Antlr4.Runtime.Tree;
using Elsa.Builders;
using Elsa.Contracts;
using Elsa.Dsl.Extensions;
using Elsa.Models;

namespace Elsa.Dsl.Interpreters
{
    public class WorkflowDefinitionBuilderInterpreter : ElsaParserBaseVisitor<IWorkflowDefinitionBuilder>
    {
        private readonly IWorkflowDefinitionBuilder _workflowDefinitionBuilder = new WorkflowDefinitionBuilder();
        private readonly ITriggerTypeRegistry _triggerTypeRegistry;
        private readonly IActivityTypeRegistry _activityTypeRegistry;
        private readonly ParseTreeProperty<object> _object = new();
        private readonly ParseTreeProperty<Type> _pairType = new();
        private readonly ParseTreeProperty<object?> _pairValue = new();

        public WorkflowDefinitionBuilderInterpreter(ITriggerTypeRegistry triggerTypeRegistry, IActivityTypeRegistry activityTypeRegistry)
        {
            _triggerTypeRegistry = triggerTypeRegistry;
            _activityTypeRegistry = activityTypeRegistry;
        }

        protected override IWorkflowDefinitionBuilder DefaultResult => _workflowDefinitionBuilder;

        public override IWorkflowDefinitionBuilder VisitTrigger(ElsaParser.TriggerContext context)
        {
            var triggerTypeName = context.@object().ID().GetText();
            var triggerType = _triggerTypeRegistry.Get(triggerTypeName);
            var trigger = (ITrigger?)Activator.CreateInstance(triggerType.Type);

            if (trigger == null)
                throw new Exception($"Could not create trigger of type {triggerTypeName}. The specified name does not exist in the trigger registry. Did you forget to register it?");
            
            _workflowDefinitionBuilder.AddTrigger(trigger);
            _object.Put(context.@object(), trigger);

            return VisitChildren(context);
        }

        public override IWorkflowDefinitionBuilder VisitObject(ElsaParser.ObjectContext context)
        {
            var @object = _object.Get(context);
            var triggerType = @object.GetType();
            var pairs = context.objectInitializer().propertyList().property();

            foreach (var pair in pairs)
            {
                var propertyName = pair.ID().GetText();
                var propertyValueExpr = pair.expr();
                var property = triggerType.GetProperty(propertyName);

                if (property == null)
                    throw new Exception($"Could not set property {propertyName} on trigger of type {triggerType.FullName}. The specified property does not exist.");

                _pairType.Put(propertyValueExpr, property.PropertyType);
                Visit(propertyValueExpr);
                var propertyValue = _pairValue.Get(propertyValueExpr);
                var propertyInputValue = CreateInputValue(property, propertyValue);
                property.SetValue(@object, propertyInputValue);
            }

            return DefaultResult;
        }
        
        public override IWorkflowDefinitionBuilder VisitBracketsExpr(ElsaParser.BracketsExprContext context)
        {
            var contents = context.exprList().expr();
            var targetCollectionType = GetUnderlyingTargetType(_pairType.Get(context));
            var targetElementType = targetCollectionType.GetGenericArguments().First();

            var items = contents.Select(x =>
            {
                Visit(x);
                return _pairValue.Get(x);
            }).ToList();
            
            var stronglyTypedListType = typeof(ICollection<>).MakeGenericType(targetElementType);
            var stronglyTypedList = items.ConvertTo(stronglyTypedListType);

            _pairValue.Put(context, stronglyTypedList);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitStringValueExpr(ElsaParser.StringValueExprContext context)
        {
            var value = context.GetText().Trim('\"');
            _pairValue.Put(context, value);
            return DefaultResult;
        }

        private Input CreateInputValue(PropertyInfo propertyInfo, object? propertyValue)
        {
            var underlyingType = propertyInfo.PropertyType.GetGenericArguments().First();
            var inputType = typeof(Input<>).MakeGenericType(underlyingType);
            var convertedValue = propertyValue.ConvertTo(underlyingType);
            var inputValue = (Input)Activator.CreateInstance(inputType, convertedValue)!;
            return inputValue;
        }

        private Type GetUnderlyingTargetType(Type type)
        {
            var targetType = typeof(Input).IsAssignableFrom(type) ? type.GetGenericArguments().First() : type;
            return targetType;
        }
    }
}