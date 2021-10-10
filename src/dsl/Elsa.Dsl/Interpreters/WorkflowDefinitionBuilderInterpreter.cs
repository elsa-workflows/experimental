using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Antlr4.Runtime.Tree;
using Elsa.Activities.Containers;
using Elsa.Builders;
using Elsa.Contracts;
using Elsa.Dsl.Extensions;
using Elsa.Dsl.Models;
using Elsa.Models;

namespace Elsa.Dsl.Interpreters
{
    public class WorkflowDefinitionBuilderInterpreter : ElsaParserBaseVisitor<IWorkflowDefinitionBuilder>
    {
        private readonly ITypeSystem _typeSystem;
        private readonly IWorkflowDefinitionBuilder _workflowDefinitionBuilder = new WorkflowDefinitionBuilder();
        private readonly ParseTreeProperty<object> _object = new();
        private readonly ParseTreeProperty<object?> _expressionValue = new();
        private readonly ParseTreeProperty<IList<object?>> _argValues = new();
        private readonly ParseTreeProperty<Type> _expressionType = new();
        private readonly IDictionary<string, DefinedVariable> _definedVariables = new Dictionary<string, DefinedVariable>();
        private readonly Stack<IContainer> _containerStack = new();

        public WorkflowDefinitionBuilderInterpreter(ITypeSystem typeSystem, WorkflowDefinitionInterpreterSettings settings)
        {
            _typeSystem = typeSystem;
        }

        protected override IWorkflowDefinitionBuilder DefaultResult => _workflowDefinitionBuilder;

        public override IWorkflowDefinitionBuilder VisitFile(ElsaParser.FileContext context)
        {
            var otherStats = context.stat().Where(x => x is not ElsaParser.ObjectStatContext).ToList();
            var objectStats = context.stat().OfType<ElsaParser.ObjectStatContext>().ToList();
            var rootActivities = objectStats.Select(x => (IActivity)_object.Get(x.@object())).ToList();
            var rootActivity = rootActivities.Count == 1 ? rootActivities.Single() : new Sequence(rootActivities.ToArray());

            _workflowDefinitionBuilder.WithRoot(rootActivity);

            if (rootActivity is IContainer container)
                _containerStack.Push(container);

            VisitMany(otherStats);
            VisitMany(objectStats);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitTrigger(ElsaParser.TriggerContext context)
        {
            VisitChildren(context);
            var trigger = (ITrigger)_object.Get(context.@object());

            _workflowDefinitionBuilder.AddTrigger(trigger);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitVarDecl(ElsaParser.VarDeclContext context)
        {
            var workflowVariableName = context.ID().GetText();

            VisitChildren(context);

            var workflowVariableValue = _expressionValue.Get(context.expr());
            
            var workflowVariable = new Variable
            {
                Name = workflowVariableName
            };
            
            if (workflowVariableValue is IActivity activity)
            {
                // When an activity is assigned to a workflow variable, what we really are doing is setting the variable to the activity's output.
                var activityType = activity.GetType();
                var outputProperty = activityType.GetProperty("Output");

                if (outputProperty == null)
                    throw new Exception("Cannot assign output of an activity that does not have an Output property.");

                var outputValue = Activator.CreateInstance(outputProperty.PropertyType, workflowVariable, default);
                outputProperty.SetValue(activity, outputValue);
            }

            var currentContainer = _containerStack.Peek();
            currentContainer.Variables.Add(workflowVariable);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitLocalVarDecl(ElsaParser.LocalVarDeclContext context)
        {
            var variableName = context.ID().GetText();
            var variableType = context.type()?.ID().GetText();

            VisitChildren(context);

            var value = _expressionValue.Get(context.expr());

            var variable = new DefinedVariable
            {
                Identifier = variableName,
                Value = value
            };

            _definedVariables[variableName] = variable;

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitObjectExpr(ElsaParser.ObjectExprContext context)
        {
            VisitChildren(context);
            var value = _expressionValue.Get(context.@object());
            _expressionValue.Put(context, value);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitObject(ElsaParser.ObjectContext context)
        {
            var objectTypeName = context.ID().GetText();
            var objectTypeDescriptor = _typeSystem.ResolveTypeName(objectTypeName);

            if (objectTypeDescriptor == null)
            {
                // Perhaps this is a variable reference?
                if (_definedVariables.TryGetValue(objectTypeName, out var definedVariable))
                {
                    _expressionValue.Put(context, definedVariable.Value);
                    return DefaultResult;
                }
                
                // Or a workflow variable?
                var workflowVariableQuery =
                    from container in _containerStack
                    from variable in container.Variables
                    where variable.Name == objectTypeName
                    select variable;

                var workflowVariable = workflowVariableQuery.FirstOrDefault();

                if (workflowVariable != null)
                {
                    _expressionValue.Put(context, workflowVariable);
                    return DefaultResult;
                }
                
                throw new Exception($"Unknown type: {objectTypeName}");
            }

            var objectType = objectTypeDescriptor.Type;
            var @object = Activator.CreateInstance(objectType)!;

            _object.Put(context, @object);
            _expressionValue.Put(context, @object);
            VisitChildren(context);

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

            _expressionType.Put(context, propertyInfo.PropertyType);
            VisitChildren(context);

            var propertyValue = _expressionValue.Get(context.expr());
            SetPropertyValue(@object, propertyInfo, propertyValue);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitBracketsExpr(ElsaParser.BracketsExprContext context)
        {
            var propertyType = _expressionType.Get(context.Parent);
            var targetElementType = propertyType.GetGenericArguments().First();
            var contents = context.exprList().expr();

            var items = contents.Select(x =>
            {
                Visit(x);
                var objectContext = x.GetChild<ElsaParser.ObjectContext>(0);
                return _expressionValue.Get(objectContext);
            }).ToList();

            var stronglyTypedListType = typeof(ICollection<>).MakeGenericType(targetElementType);
            var stronglyTypedList = items.ConvertTo(stronglyTypedListType);

            _expressionValue.Put(context, stronglyTypedList);
            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitStringValueExpr(ElsaParser.StringValueExprContext context)
        {
            var value = context.GetText().Trim('\"');
            _expressionValue.Put(context, value);
            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitNewObjectExpr(ElsaParser.NewObjectExprContext context)
        {
            VisitChildren(context);

            var value = _expressionValue.Get(context.newObject());
            _expressionValue.Put(context, value);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitNewObject(ElsaParser.NewObjectContext context)
        {
            var objectTypeName = context.ID().GetText();
            var typeArg = context.type()?.GetText();
            TypeDescriptor? typeArgTypeDescriptor = default;

            if (typeArg != null)
            {
                objectTypeName = $"{objectTypeName}<>";
                typeArgTypeDescriptor = _typeSystem.ResolveTypeName(typeArg) ?? throw new Exception($"Cannot use type {typeArg} as type argument because it was not found in the type system.");
            }

            var typeDescriptor = _typeSystem.ResolveTypeName(objectTypeName);

            if (typeDescriptor == null)
                throw new Exception($"Could not instantiate type {objectTypeName} because it was not found in the type system.");

            VisitChildren(context);

            var argValues = _argValues.Get(context.args()).ToArray();
            var objectType = typeDescriptor.Type;

            if (typeArgTypeDescriptor != null)
            {
                var objectTypeArg = typeArgTypeDescriptor.Type;
                objectType = objectType.MakeGenericType(objectTypeArg);
            }

            var objectInstance = Activator.CreateInstance(objectType, argValues);

            _expressionValue.Put(context, objectInstance);

            return DefaultResult;
        }

        public override IWorkflowDefinitionBuilder VisitArgs(ElsaParser.ArgsContext context)
        {
            var args = context.arg();

            var argValues = args.Select(x =>
            {
                Visit(x);
                return _expressionValue.Get(x.expr());
            }).ToList();

            _argValues.Put(context, argValues);

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
            var propertyValueType = propertyValue?.GetType();
            var inputType = typeof(Input<>).MakeGenericType(underlyingType);

            if (propertyValueType != null)
            {
                var hasCtorWithSpecifiedType = inputType.GetConstructors().Any(x => x.GetParameters().Any(y => y.ParameterType.IsAssignableFrom(propertyValueType)));

                if (hasCtorWithSpecifiedType)
                    return (Input)Activator.CreateInstance(inputType, propertyValue)!;
            }

            var convertedValue = propertyValue.ConvertTo(underlyingType);

            return (Input)Activator.CreateInstance(inputType, convertedValue)!;
        }

        private void VisitMany(IEnumerable<IParseTree> contexts)
        {
            foreach (var parseTree in contexts) Visit(parseTree);
        }
    }
}