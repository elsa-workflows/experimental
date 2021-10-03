using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Antlr4.Runtime.Tree;
using Elsa.Builders;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Dsl.Interpreters
{
    public class WorkflowModelInterpreter : ElsaParserBaseVisitor<IWorkflowDefinitionBuilder>
    {
        private readonly IWorkflowDefinitionBuilder _workflowDefinitionBuilder = new WorkflowDefinitionBuilder();
        private readonly ITriggerTypeRegistry _triggerTypeRegistry;
        private readonly ParseTreeProperty<ITrigger> _trigger = new();
        private readonly ParseTreeProperty<Type> _pairType = new();
        private readonly ParseTreeProperty<object?> _pairValue = new();

        public WorkflowModelInterpreter(ITriggerTypeRegistry triggerTypeRegistry)
        {
            _triggerTypeRegistry = triggerTypeRegistry;
        }

        protected override IWorkflowDefinitionBuilder DefaultResult => _workflowDefinitionBuilder;

        public override IWorkflowDefinitionBuilder VisitTrigger(ElsaParser.TriggerContext context)
        {
            var triggerTypeName = context.ID().GetText();
            var triggerType = _triggerTypeRegistry.Get(triggerTypeName);
            var trigger = (ITrigger?)Activator.CreateInstance(triggerType.Type);

            if (trigger == null)
                throw new Exception($"Could not create trigger of type {triggerTypeName}. The specified name does not exist. Did you forger to register it with the trigger registry?");
            
            _workflowDefinitionBuilder.AddTrigger(trigger);
            _trigger.Put(context.block_pairs(), trigger);

            return VisitChildren(context);
        }

        public override IWorkflowDefinitionBuilder VisitBlock_pairs(ElsaParser.Block_pairsContext context)
        {
            var trigger = _trigger.Get(context);
            var triggerType = trigger.GetType();
            var pairs = context.pairList().pair();

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
                property.SetValue(trigger, propertyInputValue);
            }

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitBrackets(ElsaParser.BracketsContext context)
        {
            var contents = context.exprList().expr();
            var targetCollectionType = GetUnderlyingTargetType(_pairType.Get(context));
            var targetElementType = targetCollectionType.GetGenericArguments().First();

            var items = contents.Select(x =>
            {
                Visit(x);
                return _pairValue.Get(x);
            }).ToList();

            // Create strongly-typed list.
            var stronglyTypedListType = typeof(ICollection<>).MakeGenericType(targetElementType);
            var stronglyTypedList = ConvertList(items, stronglyTypedListType);

            _pairValue.Put(context, stronglyTypedList);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitStringValue(ElsaParser.StringValueContext context)
        {
            var value = context.GetText().Trim('\"');
            _pairValue.Put(context, value);
            return DefaultResult;
        }

        private Input CreateInputValue(PropertyInfo propertyInfo, object? propertyValue)
        {
            var underlyingType = propertyInfo.PropertyType.GetGenericArguments().First();
            var inputType = typeof(Input<>).MakeGenericType(underlyingType);
            //var parsedValue = StringConverter.ConvertFrom(rawPropertyValue, underlyingType);
            var inputValue = (Input)Activator.CreateInstance(inputType, propertyValue)!;
            return inputValue;
        }

        private Type GetUnderlyingTargetType(Type type)
        {
            var targetType = typeof(Input).IsAssignableFrom(type) ? type.GetGenericArguments().First() : type;
            return targetType;
        }

        public static object? ConvertList(IEnumerable<object?> items, Type type)
        {
            var containedType = type.GenericTypeArguments.First();
            var enumerableType = typeof(Enumerable);
            var castMethod = enumerableType.GetMethod(nameof(Enumerable.Cast))!.MakeGenericMethod(containedType);
            var toListMethod = enumerableType.GetMethod(nameof(Enumerable.ToList))!.MakeGenericMethod(containedType);
            var castedItems = castMethod.Invoke(null, new object?[] { items });

            return toListMethod.Invoke(null!, new[] { castedItems });
        }
    }
}