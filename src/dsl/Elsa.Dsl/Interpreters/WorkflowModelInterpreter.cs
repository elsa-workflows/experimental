using System;
using System.Collections.Generic;
using System.Reflection;
using Antlr4.Runtime.Tree;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Dsl.Interpreters
{
    public class WorkflowModelInterpreter : ElsaParserBaseVisitor<Workflow>
    {
        private readonly ITriggerTypeRegistry _triggerTypeRegistry;
        private readonly ParseTreeProperty<ICollection<ITrigger>> _triggers = new();
        private readonly ParseTreeProperty<ITrigger> _trigger = new();

        public WorkflowModelInterpreter(ITriggerTypeRegistry triggerTypeRegistry)
        {
            _triggerTypeRegistry = triggerTypeRegistry;
        }

        public override Workflow VisitFile(ElsaParser.FileContext context)
        {
            var triggers = new List<ITrigger>();
            _triggers.Put(context, triggers);
            
            return VisitChildren(context);
        }

        public override Workflow VisitTrigger(ElsaParser.TriggerContext context)
        {
            var rootContext = context.Parent;
            var triggers = _triggers.Get(rootContext);
            var triggerTypeName = context.ID().GetText();
            var triggerType = _triggerTypeRegistry.Get(triggerTypeName);
            var trigger = (ITrigger?)Activator.CreateInstance(triggerType.Type);

            if (trigger == null)
                throw new Exception($"Could not create trigger of type {triggerTypeName}. The specified name does not exist. Did you forger to register it with the trigger registry?");

            triggers.Add(trigger);
            _triggers.Put(rootContext, triggers);
            _trigger.Put(context, trigger);

            return VisitChildren(context);
        }

        public override Workflow VisitBlock_pairs(ElsaParser.Block_pairsContext context)
        {
            var result = VisitChildren(context);
            
            var triggerContext = context.Parent;
            var trigger = _trigger.Get(triggerContext);
            var triggerType = trigger.GetType();
            var pairs = context.pairList().pair();

            foreach (var pair in pairs)
            {
                var propertyName = pair.ID().GetText();
                var rawPropertyValue = pair.expr().GetText();
                var property = triggerType.GetProperty(propertyName);

                if (property == null)
                    throw new Exception($"Could not set property {propertyName} on trigger of type {triggerType.FullName}. The specified property does not exist.");

                var propertyValue = CreateInputValue(property, rawPropertyValue);
                property.SetValue(trigger, propertyValue);
            }

            return result;
        }
        
        private object CreateInputValue(PropertyInfo propertyInfo, string rawPropertyValue)
        {
            var underlyingTypes = propertyInfo.PropertyType.GetGenericArguments();
            var inputType = typeof(Input<>).MakeGenericType(underlyingTypes);
            var inputValue = Activator.CreateInstance(inputType, rawPropertyValue)!;
            return inputValue;
        }
    }
}